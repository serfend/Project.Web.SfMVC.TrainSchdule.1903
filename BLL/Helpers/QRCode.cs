using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using QRCoder;
using ZXing;

namespace BLL.Helpers
{
	public class SfQrCodeConfig
	{
		public SfQrCodeConfig()
		{
			EccLevel = QRCodeGenerator.ECCLevel.M;
			LightColor = Color.White;
			DarkColor = Color.Black;
			PixelsPerModule = 10;
			IconSize = 10;
		}

		public QRCodeGenerator.ECCLevel EccLevel { get; set; }
		public Bitmap CenterIcon { get; set; }
		public int IconSize { get; set; }

		public Color LightColor { get; set; }
		public Color DarkColor { get; set; }
		public int PixelsPerModule { get; set; }
	}

	public class SfQRCoder
	{
		/// <summary>
		/// 识别图中二维码
		/// </summary>
		/// <param name="barcodeBitmap"></param>
		/// <returns></returns>
		public string DecodeQrCode(byte[] barcodeBitmap)
		{
			BarcodeReader reader = new BarcodeReader()
			{
				Options = new ZXing.Common.DecodingOptions()
				{
					CharacterSet = "utf-8"
				},
				AutoRotate = true,
				TryInverted = true
			};
			var result = reader.Decode(barcodeBitmap);
			return (result == null) ? null : result.Text;
		}

		public SfQrCodeConfig Config { get; set; }

		public SfQRCoder()
		{
			Config = new SfQrCodeConfig();
		}

		public Image Generate(string text)
		{
			using (var qrGenerator = new QRCodeGenerator())
			{
				using (var qrCodeData = qrGenerator.CreateQrCode(text, Config.EccLevel))
				{
					using (var qrCode = new QRCoder.QRCode(qrCodeData))
					{
						return qrCode.GetGraphic(Config.PixelsPerModule, Config.DarkColor, Config.LightColor,
							Config.CenterIcon, Config.IconSize);
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
			var ms = new MemoryStream();
			image.Save(ms, ImageFormat.Png);
			return ms.ToArray();
		}
	}
}