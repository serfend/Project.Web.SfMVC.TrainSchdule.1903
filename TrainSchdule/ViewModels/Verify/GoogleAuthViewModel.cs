using BLL.Interfaces;
using BLL.Services;
using System.ComponentModel.DataAnnotations;

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
		/// 超级管理
		/// </summary>
		public static readonly GoogleAuthDataModel Root = new GoogleAuthDataModel() { AuthByUserID = "root", Code = GoogleAuthService.StaticVerify };
		/// <summary>
		/// 普通用户
		/// </summary>
		public static readonly GoogleAuthDataModel User = new GoogleAuthDataModel() { AuthByUserID = "user", Code = GoogleAuthService.StaticVerify };
		/// <summary>
		/// 授权权限来源
		/// </summary>
		public string AuthByUserID { get; set; }
		/// <summary>
		/// 授权码
		/// </summary>
		[Required]
		public int Code { get; set; }
	}
	public static class GoogleAuthExtension
	{
		/// <summary>
		/// 检查当前填入的是否正确
		/// </summary>
		/// <param name="model"></param>
		/// <param name="authService"></param>
		/// <param name="currentUserId"></param>
		/// <returns></returns>
		public static bool Verify(this GoogleAuthDataModel model, IGoogleAuthService authService,string currentUserId) => model?.AuthByUserID == currentUserId||(model!=null && authService.Verify(model.Code, model?.AuthByUserID));
	}
}
