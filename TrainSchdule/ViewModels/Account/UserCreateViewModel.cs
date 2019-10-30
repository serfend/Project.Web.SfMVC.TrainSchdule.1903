using System.ComponentModel.DataAnnotations;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.Settle;
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
		public int HomeAddress { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string HomeDetailAddress { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Duties { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Company { get; set; }
		/// <summary>
		/// 随军情况
		/// </summary>

		public Settle Settle { get; set; }
	}

}
