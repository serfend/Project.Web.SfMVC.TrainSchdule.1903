using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.Settle;
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
	public class UserModefyViewModel : GoogleAuthViewModel
	{
		/// <summary>
		/// 
		/// </summary>
		public ScrollerVerifyViewModel Verify { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public UserModefyDataModel Data { get; set; }
	}
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
	public class UserMutilCreateDataModel
	{
		/// <summary>
		/// 注册账号列表
		/// </summary>
		public IEnumerable<UserCreateDataModel> List { get; set; }
	}
	public class UserCreateDataModel : UserModefyDataModel
	{
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
	/// <summary>
	/// 
	/// </summary>
	public class UserModefyDataModel
	{
		[Required]
		public UserBaseInfo Base { get; set; }
		public UserSocialDataModel Social { get; set; }
		public UserCompanyInfo Company { get; set; }
		public UserApplicationDataModel Application { get; set; }
		public UserDiyInfoDataModel Diy { get; set; }

	}
}
