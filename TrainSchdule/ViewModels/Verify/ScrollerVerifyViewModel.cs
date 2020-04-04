using BLL.Interfaces;

namespace TrainSchdule.ViewModels.Verify
{
	/// <summary>
	/// 滑动验证码
	/// </summary>
	public class ScrollerVerifyViewModel
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="_verifyService"></param>
		/// <returns></returns>
		public string Verify(IVerifyService _verifyService) => _verifyService.Verify(Code);

		/// <summary>
		/// X轴位置
		/// </summary>
		public int Code { get; set; }
	}
}