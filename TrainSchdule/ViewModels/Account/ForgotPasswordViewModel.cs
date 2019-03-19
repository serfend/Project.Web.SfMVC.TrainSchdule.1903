using System.ComponentModel.DataAnnotations;

namespace TrainSchdule.WEB.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
