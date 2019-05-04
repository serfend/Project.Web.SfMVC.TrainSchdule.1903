using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using BLL.Interfaces;
using QRCoder;

namespace BLL.Helpers
{
	public class QRCoder
	{
		public QRCodeGenerator.ECCLevel EccLevel { get; set; }
		public Bitmap CenterIcon { get; set; }
		public int IconSize { get; set; }

		public Color LightColor { get; set; }
		public Color DarkColor { get; set; }
		public int PixelsPerModule { get; set; }

		public QRCoder()
		{
			EccLevel = QRCodeGenerator.ECCLevel.M;
			LightColor = Color.White;
			DarkColor = Color.Black;
			PixelsPerModule = 10;
			IconSize = 10;
		}


		public  Image Generate(string text)
		{
			using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
			{
				using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, EccLevel))
				{
					using (QRCode qrCode = new QRCode(qrCodeData))
					{

						return qrCode.GetGraphic(PixelsPerModule, DarkColor, LightColor,
							CenterIcon, IconSize);
					}
				}
			}
		}

		public byte[] GenerateBytes(string text) => ImageToBytes(Generate(text));
		/// <summary>
		/// 图片转换为字节数组
		/// </summary>
		/// <param name="image">图片</param>
		/// <returns>字节数组</returns>
		private byte[] ImageToBytes(Image image)
		{
			MemoryStream ms = new MemoryStream();
			image.Save(ms, ImageFormat.Png);
			return ms.ToArray();
		}
	}
}
