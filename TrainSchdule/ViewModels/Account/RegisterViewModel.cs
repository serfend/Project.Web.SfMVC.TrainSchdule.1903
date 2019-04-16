using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.ViewModels.Account;
using TrainSchdule.ViewModels.Static;

namespace TrainSchdule.WEB.ViewModels.Account
{
    public class RegisterViewModel : VerifyViewModel
	{
		[Required]
		[DisplayName("auth")]
		public GoogleAuthViewModel Auth { get; set; }
        [Required]
        [StringLength(32, ErrorMessage = "{0}的长度应在{2}-{1}之间", MinimumLength = 6)]
        [Display(Name = "账号")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0}的长度应在{2}-{1}之间", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "两次输入的密码不一致")]
        public string ConfirmPassword { get; set; }
		[Display(Name = "单位")]
		public string Company { get; set; }


    }
}
