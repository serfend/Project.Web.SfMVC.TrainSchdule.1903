using BLL.Helpers;

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
		/// 导出模式 img svg etc...
		/// 默认为img
		/// </summary>
		public string ExportType { get; set; } = "img";

		/// <summary>
		/// 每个像素块对应像素数量
		/// 默认为5
		/// </summary>
		public int Size { get; set; } = 5;

		/// <summary>
		/// 图标文件名
		/// </summary>
		public IconConfiguration Icon { get; set; }

		/// <summary>
		/// 黑色部分 默认为 #000000
		/// </summary>
		public string DarkColor { get; set; } = "#000";

		/// <summary>
		/// 白色部分 默认为 #FFFFFF
		/// </summary>
		public string LightColor { get; set; } = "#FFF";

		/// <summary>
		/// 是否需要边框
		/// </summary>
		public bool Margin { get; set; } = false;

		/// <summary>
		/// 二维码图像的base64
		/// </summary>
		public string Img { get; set; }

		/// <summary>
		/// 二维码图像的svg
		/// </summary>
		public string Svg { get; set; }
	}

	/// <summary>
	/// 图标设置
	/// </summary>
	public class IconConfiguration
	{
		/// <summary>
		/// 图标文件名
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// 图标占比
		/// </summary>
		public int IconSize { get; set; } = 15;

		/// <summary>
		/// 边框占比
		/// </summary>
		public int BorderSize { get; set; } = 6;

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return $"{FileName}_{IconSize}_{BorderSize}";
		}
	}
}