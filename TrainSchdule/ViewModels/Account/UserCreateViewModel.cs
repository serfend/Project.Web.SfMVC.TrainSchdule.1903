using DAL.DTO.User;
using DAL.DTO.User.Social;
using DAL.Entities.UserInfo;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TrainSchdule.ViewModels.User;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Account
{
	/// <summary>
	/// 注册单个账号
	/// </summary>
	public class UserCreateViewModel : GoogleAuthViewModel
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
	/// 修改单个账号
	/// </summary>
	public class UserModifyViewModel : GoogleAuthViewModel
	{
		/// <summary>
		///
		/// </summary>
		public ScrollerVerifyViewModel Verify { get; set; }

		/// <summary>
		///
		/// </summary>
		public UserModifyDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UsersCreateMutilViewModel : GoogleAuthViewModel
	{
		/// <summary>
		///
		/// </summary>
		public ScrollerVerifyViewModel Verify { get; set; }

		/// <summary>
		///
		/// </summary>
		public UserMutilCreateDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserMutilCreateDataModel
	{
		/// <summary>
		/// 注册账号列表
		/// </summary>
		public IEnumerable<UserCreateDataModel> List { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserCreateDataModel : UserModifyDataModel
	{
		/// <summary>
		/// 输入密码
		/// </summary>
		[Required(ErrorMessage = "未输入密码")]
		[StringLength(32, ErrorMessage = "密码长度过短，需8位及以上", MinimumLength = 8)]
		public string Password { get; set; }

		/// <summary>
		/// 二次输入密码
		/// </summary>
		[Required(ErrorMessage = "未输入确认密码")]
		[Compare(nameof(Password), ErrorMessage = "两次输入的密码不一致")]
		public string ConfirmPassword { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserModifyDataModel
	{
		/// <summary>
		///
		/// </summary>
		[Required(ErrorMessage = "未输入基础信息")]
		public UserBaseInfo Base { get; set; }

		/// <summary>
		///
		/// </summary>
		[Required(ErrorMessage = "未输入家庭情况")]
		public SocialDto Social { get; set; }

		/// <summary>
		///
		/// </summary>
		[Required(ErrorMessage = "未输入单位信息")]
		public CompanyInfoDto Company { get; set; }

		/// <summary>
		///
		/// </summary>
		[Required(ErrorMessage = "未输入系统信息")]
		public UserApplicationDataModel Application { get; set; }

		/// <summary>
		///
		/// </summary>
		public UserDiyInfoDataModel Diy { get; set; }
	}
}