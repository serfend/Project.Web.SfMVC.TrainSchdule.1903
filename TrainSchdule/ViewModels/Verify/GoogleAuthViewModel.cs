using BLL.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace TrainSchdule.ViewModels.Verify
{
	/// <summary>
	/// 谷歌授权码
	/// </summary>
	public class GoogleAuthViewModel
	{
		[Required]
		public GoogleAuthDataModel Auth { get; set; }
	}
	public class GoogleAuthDataModel
	{
		/// <summary>
		/// 授权权限来源
		/// </summary>
		[Required]
		public string AuthByUserID { get; set; }
		/// <summary>
		/// 授权码
		/// </summary>
		[Required]
		public int Code { get; set; }
	}
	public static class GoogleAuthExtension
	{
		public static bool Verify(this GoogleAuthDataModel model, IGoogleAuthService authService) => authService.Verify(model.Code, model?.AuthByUserID);
	}
}
