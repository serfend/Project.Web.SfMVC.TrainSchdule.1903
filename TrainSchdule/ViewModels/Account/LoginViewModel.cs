using System.ComponentModel.DataAnnotations;

namespace TrainSchdule.WEB.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(32, ErrorMessage = "非法的{0}", MinimumLength = 6)]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "记住登录")]
        public bool RememberMe { get; set; }

		[Display(Name = "验证码")]
		public int Verify { get; set; }
    }
}
