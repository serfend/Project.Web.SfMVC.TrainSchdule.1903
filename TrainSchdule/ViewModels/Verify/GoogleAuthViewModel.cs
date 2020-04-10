using BLL.Interfaces;
using BLL.Services;
using System;

namespace TrainSchdule.ViewModels.Verify
{
	/// <summary>
	/// 谷歌授权码
	/// </summary>
	public class GoogleAuthViewModel
	{
		/// <summary>
		/// 授权
		/// </summary>
		public GoogleAuthDataModel Auth { get; set; }
	}

	/// <summary>
	/// 授权
	/// </summary>
	public class GoogleAuthDataModel
	{
		/// <summary>
		/// 授权权限来源
		/// </summary>
		public string AuthByUserID { get; set; }

		/// <summary>
		/// 授权码
		/// </summary>
		public string Code { get; set; }
	}

	/// <summary>
	/// 认证
	/// </summary>
	public static class GoogleAuthExtension
	{
		/// <summary>
		/// 检查当前填入的是否正确
		/// </summary>
		/// <param name="model"></param>
		/// <param name="authService"></param>
		/// <param name="currentUserId"></param>
		/// <returns></returns>
		public static bool Verify(this GoogleAuthDataModel model, IGoogleAuthService authService, string currentUserId) => model?.AuthByUserID == currentUserId || (model != null && authService.Verify(Convert.ToInt32(model.Code), model?.AuthByUserID));
	}
}