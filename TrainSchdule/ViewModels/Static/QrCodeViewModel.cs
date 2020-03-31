using BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Static
{
	/// <summary>
	///
	/// </summary>
	public class QrCodeViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public QrCodeDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>

	public class QrCodeDataModel
	{
		/// <summary>
		/// 二维码原始内容
		/// </summary>
		public string Data { get; set; }

		/// <summary>
		/// 二维码图像
		/// </summary>
		public byte[] Img { get; set; }

		/// <summary>
		/// 二维码设置
		/// </summary>

		public SfQrCodeConfig Config { get; set; }
	}
}