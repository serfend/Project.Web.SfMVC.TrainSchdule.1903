using System.ComponentModel.DataAnnotations;

namespace TrainSchdule.WEB.ViewModels.Account
{
    public class LoginWith2faViewModel
    {
        [Required]
        [StringLength(7, ErrorMessage = "{0}的长度必须在{1}到{2}之间", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "授权码")]
        public string TwoFactorCode { get; set; }

        [Display(Name = "下次自动登录")]
        public bool RememberMachine { get; set; }

        public bool RememberMe { get; set; }
    }
}
