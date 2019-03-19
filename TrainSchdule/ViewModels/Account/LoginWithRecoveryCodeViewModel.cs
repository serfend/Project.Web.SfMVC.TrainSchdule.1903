using System.ComponentModel.DataAnnotations;

namespace TrainSchdule.WEB.ViewModels.Account
{
    public class LoginWithRecoveryCodeViewModel
    {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Recovery Code")]
            public string RecoveryCode { get; set; }
    }
}
