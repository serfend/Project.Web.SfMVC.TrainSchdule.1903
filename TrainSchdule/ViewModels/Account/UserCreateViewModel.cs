using System;
using System.ComponentModel.DataAnnotations;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.Settle;
using TrainSchdule.ViewModels.User;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Account
{
	/// <summary>
	/// 
	/// </summary>
	public class UserCreateViewModel: GoogleAuthViewModel
	{
		/// <summary>
		/// 
		/// </summary>
		public ScrollerVerifyViewModel Verify { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public UserCreateDataModel Data { get; set; }
	}

	/// <summary>
	/// 
	/// </summary>
	public class UserCreateDataModel
	{
		[Required]
		public UserBaseInfo Base { get; set; }
		public UserSocialDataModel Social{ get; set; }
		public UserCompanyInfo Company { get; set; }
		public UserApplicationDataModel Application { get; set; }

		/// <summary>
		/// 输入密码
		/// </summary>
		[Required]
		[StringLength(32, ErrorMessage = "非法的{0}", MinimumLength = 8)]

		public string Password { get; set; }
		/// <summary>
		/// 二次输入密码
		/// </summary>
		[Required]
		[Compare(nameof(Password), ErrorMessage = "两次输入的密码不一致")]
		public string ConfirmPassword { get; set; }
	}

}
