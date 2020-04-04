using BLL.Helpers;
using DAL.Entities.BBS;

namespace TrainSchdule.ViewModels.BBS
{
	/// <summary>
	///
	/// </summary>
	public class SignInSuccessViewModel : ApiResult
	{/// <summary>
	 ///
	 /// </summary>
		public SignInSuccessDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class SignInSuccessDataModel
	{
		/// <summary>
		/// 签到成功描述
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 签到
		/// </summary>
		public SignIn SignIn { get; set; }
	}
}