using BLL.Extensions;
using BLL.Helpers;
using DAL.Entities.FileEngine;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainSchdule.Extensions.Common;
using TrainSchdule.ViewModels.Static;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace TrainSchdule.Controllers
{
	/// <summary>
	///
	/// </summary>
	public partial class StaticController
	{
		private const string QRCodePath = "QRCodeGen";

		private ApiResult GenerateQrCodeImg(QrCodeDataModel model, out UserFileInfo jpgFile, out UserFileInfo svgFile)
		{
			var fileNameRaw = $"{model.Data}_{model.Margin}_{model.Icon}_{model.LightColor}_{model.DarkColor}_{model.Size}";
			var fileName = $"{fileNameRaw.ToMd5()}";
			var imgFileName = $"{fileName}.jpg";
			var svgFileName = $"{fileName}.svg";
			UserFile iconFileRaw = null;
			if (model.Icon?.FileName != null)
			{
				Guid.TryParse(model.Icon.FileName, out var fid);
				if (fid != null)
				{
					iconFileRaw = _fileServices.Download(fid);
				}
			}

			Bitmap icon = null;
			if (iconFileRaw != null)
			{
				using (var iconMs = new MemoryStream(iconFileRaw.Data))
				{
					icon = new Bitmap(iconMs);
				}
			}
			jpgFile = _fileServices.Load(QRCodePath, imgFileName);
			svgFile = _fileServices.Load(QRCodePath, svgFileName);
			if (jpgFile != null && svgFile != null) return null;
			if (model == null) return (ActionStatusMessage.Static.QrCode.NoData);
			var rawText = model.Data;
			if (rawText == null)
				return (ActionStatusMessage.Static.QrCode.NoData);
			var qrCodeData = new QRCoder.QRCodeGenerator().CreateQrCode(rawText, QRCodeGenerator.ECCLevel.H);
			var svg = new SvgQRCode(qrCodeData).GetGraphic(model.Size, model.DarkColor, model.LightColor, true, SvgQRCode.SizingMode.ViewBoxAttribute);
			var bitmap = new QRCode(qrCodeData).GetGraphic(model.Size, ColorTranslator.FromHtml(model.DarkColor), ColorTranslator.FromHtml(model.LightColor), icon, model.Icon?.IconSize ?? 15, model.Icon?.BorderSize ?? 6, model?.Margin ?? false);

			if (jpgFile == null)
				using (MemoryStream stream = new MemoryStream())
				{
					bitmap.Save(stream, ImageFormat.Jpeg);
					var imgf = new FormFile(stream, 0, stream.Length, imgFileName, imgFileName);
					jpgFile = _fileServices.Upload(imgf, QRCodePath, imgFileName, Guid.Empty, Guid.Empty).Result;
				}
			if (svgFile == null)
				using (var sr = new MemoryStream())
				{
					sr.Write(Encoding.UTF8.GetBytes(svg));
					var svgf = new FormFile(sr, 0, sr.Length, svgFileName, svgFileName);
					svgFile = _fileServices.Upload(svgf, QRCodePath, svgFileName, Guid.Empty, Guid.Empty).Result;
				}
			return null;
		}

		/// <summary>
		/// 产生一个二维码图像
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		public IActionResult QrCodeGenerateImg([FromBody] QrCodeDataModel model)
		{
			var result = GenerateQrCodeImg(model, out var img, out var svg);
			if (result != null) return new JsonResult(result);
			return Redirect(img.DownloadUrl(FileExtensions.DownloadType.ByStatic));
		}

		/// <summary>
		/// 产生一个二维码
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		public IActionResult QrCodeGenerate([FromBody] QrCodeDataModel model)
		{
			var result = GenerateQrCodeImg(model, out var img, out var svg);
			if (result != null) return new JsonResult(result);
			return new JsonResult(new QrCodeViewModel()
			{
				Data = new QrCodeDataModel()
				{
					Data = model.Data,
					Img = img.DownloadUrl(FileExtensions.DownloadType.ByStatic),
					Svg = svg.DownloadUrl(FileExtensions.DownloadType.ByStatic)
				}
			});
		}

		/// <summary>
		/// 识别二维码
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		public IActionResult QrCodeScan([FromBody] QrCodeDataModel model)
		{
			if (model == null) return new JsonResult(ActionStatusMessage.Static.QrCode.NoData);

			var imgRaw = Convert.FromBase64String(model.Img);
			using (var ms = new MemoryStream(imgRaw))
			{
				var image = Image.FromStream(ms);
				var bitmap = new HybridBinarizer(new RGBLuminanceSource(imgRaw, image.Width, image.Height));
				return new JsonResult(new QrCodeViewModel()
				{
					Data = new QrCodeDataModel()
					{
						Data = new ZXing.QrCode.QRCodeReader().decode(new BinaryBitmap(bitmap)).Text
					}
				});
			}
		}
	}
}