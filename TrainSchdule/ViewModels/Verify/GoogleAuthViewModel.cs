using BLL.Extensions;
using BLL.Helpers;
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
		public static bool Verify(this GoogleAuthDataModel model, IGoogleAuthService authService, string currentUserId) => model?.AuthByUserID == currentUserId || (model != null && authService.Verify(Convert.ToInt32(model?.Code ?? "0"), model?.AuthByUserID));

		/// <summary>
		/// 获取授权人
		/// </summary>
		/// <param name="model"></param>
		/// <param name="authService"></param>
		/// <param name="currentUserId"></param>
		/// <returns></returns>
		public static string AuthUser(this GoogleAuthDataModel model, IGoogleAuthService authService, string currentUserId)
		{
			var result = currentUserId;
			if (model?.AuthByUserID != null)
			{
				if (!model.Verify(authService, result)) throw new ActionStatusMessageException(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
				result = model.AuthByUserID;
			}
			if (result == null) throw new ActionStatusMessageException(ActionStatusMessage.Account.Auth.Permission.AuthUserNotSet);
			return result;
		}

		/// <summary>
		/// 获取授权人本人
		/// </summary>
		/// <param name="model"></param>
		/// <param name="authService"></param>
		/// <param name="usersService"></param>
		/// <param name="currentUserId"></param>
		/// <returns></returns>
		public static DAL.Entities.UserInfo.User AuthUser(this GoogleAuthDataModel model, IGoogleAuthService authService, IUsersService usersService, string currentUserId)
		{
			var u = model.AuthUser(authService, currentUserId);
			var user = usersService.GetById(u);
			if (user == null) throw new ActionStatusMessageException(ActionStatusMessage.Account.Auth.Permission.AuthUserNotExist);
			return user;
		}

        /// <summary>
        /// 授权失败
        /// </summary>
        /// <param name="model"></param>
        /// <param name="appendMessage"></param>
        /// <returns></returns>
        public static ApiResult PermitDenied(this GoogleAuthDataModel model, string appendMessage = null) => appendMessage != null ? new ApiResult(ActionStatusMessage.Account.Auth.Invalid.Default, appendMessage, true) : ActionStatusMessage.Account.Auth.Invalid.Default;
	}
}