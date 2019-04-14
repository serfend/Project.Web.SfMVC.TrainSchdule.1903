using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.DrawingCore;
using System.DrawingCore.Drawing2D;
using System.DrawingCore.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Memory;
namespace BLL.Services
{
	public class VerifyService:IVerifyService
	{
		#region Fileds
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IFileProvider _fileProvider;

		#endregion
		#region Disposing

		private bool _isDisposed;

		public VerifyService( IHttpContextAccessor httpContextAccessor, IFileProvider fileProvider)
		{
			_httpContextAccessor = httpContextAccessor;
			_fileProvider = fileProvider;
			ReloadPath();
			_cache = new MemoryCache(new MemoryCacheOptions()
			{
				ExpirationScanFrequency = TimeSpan.FromMinutes(30)
			});
		}

		private void ReloadPath()
		{
			verifyImgNum = _fileProvider.GetDirectoryContents(verifyPath).Count();
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				if (disposing)
				{
					
				}

				_isDisposed = true;
			}
		}

		~VerifyService()
		{
			Dispose(false);
		}

		#endregion

		private const string KEY_VerifyCode = "verify-code";


		private static int verifyImgNum = 0;
		private static int RndIndex => new Random().Next(0, verifyImgNum);

		public byte[] Front()
		{
			return GetImg()?.Front;
		}

		public byte[] Background()
		{
			return GetImg()?.Background;
		}

		private const string verifyPath = "wwwroot\\images\\verify";
		private readonly MemoryCache _cache;
		public Guid Generate()
		{
			var newCodeValue = Guid.NewGuid();
			_httpContextAccessor.HttpContext.Session.Set(KEY_VerifyCode, Encoding.UTF8.GetBytes(newCodeValue.ToString()));
			int index = RndIndex;
			var file=_fileProvider.GetDirectoryContents(verifyPath).Skip(index)?.FirstOrDefault();
			if (file == null)
			{
				ReloadPath();
			}
			else
			{
				using (var sr = file.CreateReadStream())
				{
					_cache.Set(newCodeValue.ToString(), new VerifyImg(Image.FromStream(sr)));
				}
			}

			return newCodeValue;
		}

		private VerifyImg GetImg(bool autoGenerate=true)
		{
			Status = string.Empty;
			_httpContextAccessor.HttpContext.Session.TryGetValue(KEY_VerifyCode, out var codeIndex);
			if (codeIndex == null)
			{
				codeIndex=Encoding.UTF8.GetBytes(Generate().ToString());
			}
			var obj=_cache.Get(Encoding.UTF8.GetString(codeIndex));
			VerifyImg img = (VerifyImg) obj;
			if(img==null)Status = "验证码已过期";
			return img;
		}
		public bool Verify(int code)
		{
			var img = GetImg();
			if (img == null) return false;
			return img.Verify(code);
		}

		public string Status { get; private set; }
	}

	public class VerifyImg
	{
		public byte[] Front;
		public byte[] Background;
		private int code;
		public bool Verify(int code)
		{
			return Math.Abs(code - this.code) < 20;
		}


		public VerifyImg(Image raw)
		{
			Graphics g=Graphics.FromImage(raw);
			var size = raw.Size;
			int width = (int) (size.Height * 0.3);
			int percentWidth=(int)((double)(width * 1000) / ((double)size.Width));
			var imgChild = new Bitmap(width, width);
			Graphics gchild=Graphics.FromImage(imgChild);
			this.code = new Random().Next(50, 1000- percentWidth);
			int top = (int) (size.Height * (0.6*new Random().NextDouble()+0.1));
			int left = (this.code * size.Width) / 1000;

			//创建剪影
			var gp=new GraphicsPath(FillMode.Winding);
			gp.StartFigure();
			float r = (float) (width * 0.5);
			PointF rightEllipsePos=new PointF((float)(width-r*0.5), (float)(width*0.5 - r*0.5));//右侧中间的小圈
			PointF bottomEllipsePos = new PointF((float)(width*0.5 - r*0.5), (float)(width - r*0.5));//下侧中间的小圈
			gp.AddEllipse(rightEllipsePos.X, rightEllipsePos.Y,r,r);
			gp.AddEllipse(bottomEllipsePos.X, bottomEllipsePos.Y,r,r);
			gp.AddRectangle(new Rectangle(0,0,width,width));
			gp.CloseAllFigures();
			var matrix = new Matrix();
			matrix.Translate(left, top);
			gp.Transform(matrix);
			g.FillPath(new SolidBrush(Color.FromArgb(200,0,0,0)),gp );
			//g.FillPath(new TextureBrush(raw), gp);
			g.FillPath(new PathGradientBrush(gp)
			{
				SurroundColors = new Color[] { Color.FromArgb(100, 0, 0, 0), Color.FromArgb(0, 0, 0, 0) },
			}, gp);

			var srcRect = new Rectangle(left, top, width, width);
			gchild.Clip=new Region(new Rectangle(0,0, srcRect.Width, srcRect.Height)); 
			gchild.FillPath(new TextureBrush(imgChild), gp);

			Front = ImageToBytes(imgChild) ;
			Background = ImageToBytes(raw);
		}
		/// <summary>
		/// 图片转换为字节数组
		/// </summary>
		/// <param name="image">图片</param>
		/// <returns>字节数组</returns>
		private byte[] ImageToBytes(Image image)
		{
			MemoryStream ms = new MemoryStream();
			image.Save(ms,ImageFormat.Png);
			return ms.ToArray();
		}
	}
}
