
using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using System;
using System.DrawingCore;
using System.DrawingCore.Drawing2D;
using System.DrawingCore.Imaging;
using System.IO;
using System.Linq;
using System.Text;
namespace BLL.Services
{
	public class VerifyService : IVerifyService
	{
		#region Fileds
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IFileProvider _fileProvider;
		public static readonly int StaticVerify = 201700816;
		#endregion

		public VerifyService(IHttpContextAccessor httpContextAccessor, IFileProvider fileProvider)
		{
			_httpContextAccessor = httpContextAccessor;
			_fileProvider = fileProvider;
			ReloadPath();
			_cache = new MemoryCache(new MemoryCacheOptions()
			{
				ExpirationScanFrequency = TimeSpan.FromMinutes(30)
			});
		}
		~VerifyService()
		{
			_cache.Dispose();
		}
		private void ReloadPath()
		{
			verifyImgNum = _fileProvider.GetDirectoryContents(VerifyPath).Count();
		}

		private const string KeyVerifyCode = "verify-code";

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

		private const string VerifyPath = "wwwroot\\images\\verify";
		private readonly MemoryCache _cache;
		public Guid Generate()
		{
			var newCodeValue = Guid.NewGuid();
			_httpContextAccessor.HttpContext.Session.Set(KeyVerifyCode, Encoding.UTF8.GetBytes(newCodeValue.ToString()));
			var index = RndIndex;
			var file = _fileProvider.GetDirectoryContents(VerifyPath).Skip(index)?.FirstOrDefault();
			if (file == null)
			{
				ReloadPath();
			}
			else
			{
				using (var sr = file.CreateReadStream())
				{
					var img = new VerifyImg(Image.FromStream(sr));
					Pos = new Point(img.X, img.Y);
					_cache.Set(newCodeValue.ToString(), img);
				}
			}

			return newCodeValue;
		}

		private VerifyImg GetImg()
		{
			Status = string.Empty;
			_httpContextAccessor.HttpContext.Session.TryGetValue(KeyVerifyCode, out var codeIndex);
			if (codeIndex == null)
			{
				codeIndex = Encoding.UTF8.GetBytes(Generate().ToString());
			}
			var obj = _cache.Get(Encoding.UTF8.GetString(codeIndex));
			var img = (VerifyImg)obj;
			if (img == null) Status = "验证码已过期";
			return img;
		}
		public string Verify(int code)
		{
			string result;
			var img = GetImg();
			if (img == null)
			{
				result = "验证码未初始化";
			}
			else result = img.Verify(code);

			Generate();
			return result;
		}

		public string Status { get; private set; }
		public Point Pos { get; private set; }
	}

	public class VerifyImg
	{
		private byte[] front;
		private byte[] background;
		private int _code;
		public int X => _code;
		public int Y { get; private set; }
		public byte[] Front { get => front; set => front = value; }
		public byte[] Background { get => background; set => background = value; }

		public string Verify(int code)
		{
			bool success = VerifyService.StaticVerify == code || Math.Abs(code - _code) < 5;
			return success ? "" : $"验证码错误 your x={code} except x={_code}";
		}

		private static Image Compress(Image raw, int newWidth)
		{
			var size = raw.Size;
			var dstWidth = (double)newWidth;
			double dstHeight = (int)(dstWidth * size.Height / size.Width);

			var img = new Bitmap((int)dstWidth, (int)dstHeight);
			var g = Graphics.FromImage(img);
			g.DrawImage(raw, new RectangleF(0, 0, (float)dstWidth, (float)dstHeight), new RectangleF(0, 0, raw.Width, raw.Height), GraphicsUnit.Pixel);
			return img;
		}
		/// <summary>
		/// 对图片增加拼图的剪切路径
		/// </summary>
		private static void PatchMapPath(Graphics front, Graphics back, RectangleF src, Image srcImg)
		{
			var width = src.Width;
			var height = src.Height;
			var left = src.Left;
			var top = src.Top;
			//创建剪影
			var gp = new GraphicsPath(FillMode.Winding);
			gp.StartFigure();
			var r = (float)(width * 0.2);

			/*     H
			   A|------|
			   B）	   |  G
			   C|__  __|
				 D ∪ F
				   E
			*/

			//A
			gp.AddLine(left, top, left, (float)(top + width * 0.5 - r));

			//B
			gp.AddArc((float)(left - r), (float)(top + width * 0.5 - r), (float)(r * 2f), r * 2f, -90f, 180f);

			//C
			gp.AddLine(left, (float)(top + width * 0.5 + r), left, (float)(top + width));

			//D
			gp.AddLine(left, top + width, (float)(left + width * 0.5 - r), top + width);

			//E
			gp.AddArc((float)(left + width * 0.5 - r), (float)(top + width - r), r * 2f, r * 2f, 0f, 180f);

			//F
			gp.AddLine((float)(left + width * 0.5 + r), top + width, left + width, top + width);

			//G
			gp.AddLine(left + width, top + width, left + width, top);

			//H
			gp.AddLine(left + width, top, left, top);
			gp.CloseFigure();


			using (var sb = new SolidBrush(Color.FromArgb(200, 0, 0, 0)))
			{
				using (var pg = new PathGradientBrush(gp)
				{
					SurroundColors = new Color[] { Color.FromArgb(100, 0, 0, 0), Color.FromArgb(0, 0, 0, 0) },
				})
				{
					back.FillPath(sb, gp);
					back.FillPath(pg, gp);
					front.FillPath(new TextureBrush(srcImg), gp);
				}
			}



		}

		private void InitCodeValue(int fullWidth, int targetWidth)
		{
			_code = new Random().Next(50, fullWidth - targetWidth);
		}
		public VerifyImg(Image raw)
		{
			var imgBack = Compress(raw, 260);
			var size = imgBack.Size;
			var width = (int)(size.Height * 0.3);
			InitCodeValue(size.Width, width);


			var imgFront = new Bitmap(width, width);


			var gBack = Graphics.FromImage(imgBack);
			var gFront = Graphics.FromImage(imgFront);
			gBack.SmoothingMode = SmoothingMode.AntiAlias;
			gFront.SmoothingMode = SmoothingMode.AntiAlias;
			var top = (int)(size.Height * (0.6 * new Random().NextDouble() + 0.1));
			Y = top;
			var left = (int)(_code);

			//

			var srcRect = new Rectangle(left, top, width, width);

			gFront.DrawImage(imgBack, new RectangleF(0, 0, width, width), srcRect, GraphicsUnit.Pixel);
			PatchMapPath(gFront, gBack, srcRect, imgBack);

			Front = ImageToBytes(imgFront);
			Background = ImageToBytes(imgBack);
		}
		/// <summary>
		/// 图片转换为字节数组
		/// </summary>
		/// <param name="image">图片</param>
		/// <returns>字节数组</returns>
		private static byte[] ImageToBytes(Image image)
		{
			var ms = new MemoryStream();
			image.Save(ms, ImageFormat.Jpeg);
			return ms.ToArray();
		}
	}
}
