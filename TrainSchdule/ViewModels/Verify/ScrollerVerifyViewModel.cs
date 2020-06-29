using BLL.Helpers;
using BLL.Interfaces;

namespace TrainSchdule.ViewModels.Verify
{
	/// <summary>
	/// 滑动验证码
	/// </summary>
	public class ScrollerVerifyViewModel
	{
		/// <summary>
		/// X轴位置
		/// </summary>
		public int Code { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public static class ScrollerVerifyExtensions
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <param name="_verifyService"></param>
		/// <returns></returns>
		public static void Verify(this ScrollerVerifyViewModel model, IVerifyService _verifyService)
		{
			if (model == null) throw new ActionStatusMessageException(ActionStatusMessage.Account.Auth.Verify.NotSet);
			var result = _verifyService.Verify(model.Code);
			if (result != null) throw new ActionStatusMessageException(new ApiResult(ActionStatusMessage.Account.Auth.Verify.Invalid, result, true));
		}
	}
}