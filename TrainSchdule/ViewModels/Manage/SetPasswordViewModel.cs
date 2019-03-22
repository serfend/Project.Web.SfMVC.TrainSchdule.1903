using System.ComponentModel.DataAnnotations;

namespace TrainSchdule.WEB.ViewModels.Manage
{
    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0}密码长度应在{1}-{2}之间", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新的密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [Compare(nameof(NewPassword), ErrorMessage = "两次输入的密码不一致")]
        public string ConfirmPassword { get; set; }

        public string StatusMessage { get; set; }
    }
}
