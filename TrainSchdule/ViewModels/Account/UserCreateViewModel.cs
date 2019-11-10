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
	public class UserCreateViewModel
	{
		/// <summary>
		/// 
		/// </summary>
		public ScrollerVerifyViewModel Verify { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public GoogleAuthViewModel Auth { get; set; }
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
		/// <summary>
		/// 
		/// </summary>
		[Required]
		public string Id { get; set; }
		/// <summary>
		/// 身份证号
		/// </summary>
		[Required]
		public string Cid { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[Required]
		public string RealName { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[Required]
		[StringLength(32, ErrorMessage = "非法的{0}", MinimumLength = 8)]
	
		public string Password { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[Required]
		[Compare(nameof(Password), ErrorMessage = "两次输入的密码不一致")]
		public string ConfirmPassword { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Required]
		public string Phone { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Email { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Duties { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Company { get; set; }
		/// <summary>
		/// 个人家庭情况
		/// </summary>

		public SettleDataModel Settle { get; set; }
		/// <summary>
		/// 工作/入伍时间
		/// </summary>
		public DateTime Time_Work { get; set; }
		/// <summary>
		/// 出生日期
		/// </summary>
		public DateTime Time_BirthDay { get; set; }
		/// <summary>
		/// 党团时间
		/// </summary>
		public DateTime Time_Party { get; set; }
	}

}
