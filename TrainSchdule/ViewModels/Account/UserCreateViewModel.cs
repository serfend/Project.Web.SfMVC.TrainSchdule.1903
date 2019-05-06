using System.ComponentModel.DataAnnotations;
using DAL.DTO.User;
using DAL.Entities.UserInfo;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Account
{
	public class UserCreateViewModel
	{
		public ScrollerVerifyViewModel Verify { get; set; }
		public GoogleAuthViewModel Auth { get; set; }
		public UserCreateDataModel Data { get; set; }
	}

	public class UserCreateDataModel
	{
		[Required]
		public string Id { get; set; }
		[Required]
		public string RealName { get; set; }
		public GenderEnum Gender { get; set; }
		[Required]
		[StringLength(32, ErrorMessage = "非法的{0}", MinimumLength = 8)]
		public string Password { get; set; }
		[Required]
		[Compare(nameof(Password), ErrorMessage = "两次输入的密码不一致")]
		public string ConfirmPassword { get; set; }
		[Required]
		public string Phone { get; set; }
		public string Email { get; set; }
		public int HomeAddress { get; set; }
		public string HomeDetailAddress { get; set; }
		public string Duties { get; set; }
		public string Company { get; set; }
	}

}
