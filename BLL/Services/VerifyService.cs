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
					var img = new VerifyImg(Image.FromStream(sr));
					Pos=new Point(img.X,img.Y);
					_cache.Set(newCodeValue.ToString(), img);
				}
			}

			return newCodeValue;
		}

		private VerifyImg GetImg()
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
			bool result;
			var img = GetImg();
			if (img == null)
			{
				result= false;
			}else result=img.Verify(code);

			Generate();
			return result;
		}

		public string Status { get; private set; }
		public Point Pos { get; private set; }
	}

	public class VerifyImg
	{
		public byte[] Front;
		public byte[] Background;
		private int code;
		public int X { get=>code; }
		public int Y { get; private set; }
		public bool Verify(int code)
		{
			return 201700816==code||Math.Abs(code - this.code) < 20;
		}

		private Image Compress(Image raw, int newWidth)
		{
			var size = raw.Size;
			double dstWidth = (double)newWidth;
			double dstHeight = (int)(dstWidth * size.Height / size.Width);

			var img = new Bitmap((int)dstWidth, (int)dstHeight);
			Graphics g = Graphics.FromImage(img);
			g.DrawImage(raw, new RectangleF(0, 0, (float)dstWidth, (float)dstHeight), new RectangleF(0, 0, raw.Width, raw.Height), GraphicsUnit.Pixel);
			return img;
		}
		/// <summary>
		/// 对图片增加拼图的剪切路径
		/// </summary>
		private void PatchMapPath(Graphics front,Graphics back,RectangleF src,Image srcImg)
		{
			float width = src.Width;
			float height = src.Height;
			float left = src.Left;
			float top = src.Top;
			//创建剪影
			var gp = new GraphicsPath(FillMode.Winding);
			gp.StartFigure();
			float r = (float)(width * 0.2);

			/*     H
			   A|------|
			   B）	   |  G
			   C|__  __|
				 D ∪ F
				   E
			*/

			//A
			gp.AddLine(left, top, left, (float)(top + width * 0.5- r));

			//B
			gp.AddArc((float)(left - r),(float)(top +width * 0.5 - r),(float)(r * 2f),r*2f,-90f,180f);

			//C
			gp.AddLine(left, (float)(top + width * 0.5+r), left, (float)(top + width));

			//D
			gp.AddLine(left, top + width, (float)(left + width * 0.5-r), top + width);

			//E
			gp.AddArc((float)(left+width*0.5 - r), (float)(top + width - r), r * 2f, r * 2f, 0f, 180f);

			//F
			gp.AddLine((float)(left + width * 0.5+r), top + width, left + width, top + width);

			//G
			gp.AddLine(left+width,top+width,left+width,top);

			//H
			gp.AddLine(left + width, top , left, top);
			gp.CloseFigure() ;



			back.FillPath(new SolidBrush(Color.FromArgb(200, 0, 0, 0)), gp);
			back.FillPath(new PathGradientBrush(gp)
			{
				SurroundColors = new Color[] { Color.FromArgb(100, 0, 0, 0), Color.FromArgb(0, 0, 0, 0) },
			}, gp);

			//var matrix = new Matrix();
			//matrix.Translate(-left, -top);
			//gp.Transform(matrix);
			front.FillPath(new TextureBrush(srcImg), gp);
			

		}

		private void InitCodeValue(int fullWidth,int targetWidth)
		{
			this.code = new Random().Next(50, fullWidth - targetWidth);
		}
		public VerifyImg(Image raw)
		{
			var imgBack = Compress(raw,360);
			var size = imgBack.Size;
			int width = (int)(size.Height * 0.3);
			InitCodeValue(size.Width, width);


			var imgFront = new Bitmap(width, width);

			 
			Graphics gBack =Graphics.FromImage(imgBack);
			Graphics gFront = Graphics.FromImage(imgFront);
			gBack.SmoothingMode = SmoothingMode.AntiAlias;
			gFront.SmoothingMode = SmoothingMode.AntiAlias;
			int top = (int) (size.Height * (0.6*new Random().NextDouble()+0.1));
			Y = top;
			int left = (int)(((double)this.code * size.Width) / 1000);

			//

			var srcRect = new Rectangle(left, top, width, width);

			gFront.DrawImage(imgBack, new RectangleF(0, 0, width, width), srcRect, GraphicsUnit.Pixel);
			PatchMapPath(gFront, gBack, srcRect,imgBack);

			Front = ImageToBytes(imgFront) ;
			Background = ImageToBytes(imgBack);
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
