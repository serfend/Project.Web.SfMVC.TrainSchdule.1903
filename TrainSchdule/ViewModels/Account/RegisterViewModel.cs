using System.ComponentModel.DataAnnotations;

namespace TrainSchdule.WEB.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(32, ErrorMessage = "{0}的长度应在{2}-{1}之间", MinimumLength = 6)]
        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0}的长度应在{2}-{1}之间", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "两次输入的密码不一致")]
        public string ConfirmPassword { get; set; }
    }
}
