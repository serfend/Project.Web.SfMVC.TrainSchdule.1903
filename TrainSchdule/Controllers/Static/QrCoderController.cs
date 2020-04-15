using BLL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Static;

namespace TrainSchdule.Controllers.Static
{
	/// <summary>
	///
	/// </summary>
	public partial class StaticController
	{
		private ApiResult GenerateQrCode(QrCodeDataModel model, out byte[] img)
		{
			img = null;
			if (model == null) return (ActionStatusMessage.Static.QrCode.NoData);
			var rawText = model.Data;
			var qrEncoder = new MessagingToolkit.QRCode.Codec.QRCodeEncoder();
			if (rawText == null)
				return (ActionStatusMessage.Static.QrCode.NoData);
			var bitmap = qrEncoder.Encode(rawText);
			using (MemoryStream stream = new MemoryStream())
			{
				bitmap.Save(stream, ImageFormat.Jpeg);
				img = new byte[stream.Length];
				stream.Seek(0, SeekOrigin.Begin);
				stream.Read(img, 0, Convert.ToInt32(stream.Length));
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
			var result = GenerateQrCode(model, out var img);
			if (result != null) return new JsonResult(result);
			return new FileContentResult(img, "image/png");
		}

		/// <summary>
		/// 产生一个二维码
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		public IActionResult QrCodeGenerate([FromBody] QrCodeDataModel model)
		{
			var result = GenerateQrCode(model, out var img);
			if (result != null) return new JsonResult(result);
			return new JsonResult(new QrCodeViewModel()
			{
				Data = new QrCodeDataModel()
				{
					Data = model.Data,
					Img = Convert.ToBase64String(img)
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
			var qrEncoder = new MessagingToolkit.QRCode.Codec.QRCodeDecoder();
			var imgRaw = Convert.FromBase64String(model.Img);
			using (var ms = new MemoryStream(imgRaw))
			{
				var image = Image.FromStream(ms);
				var img = new MessagingToolkit.QRCode.Codec.Data.QRCodeBitmapImage((Bitmap)image);
				return new JsonResult(new QrCodeViewModel()
				{
					Data = new QrCodeDataModel()
					{
						Data = qrEncoder.Decode(img)
					}
				});
			}
		}
	}
}