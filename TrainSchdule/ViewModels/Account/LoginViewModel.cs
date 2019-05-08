using System.ComponentModel.DataAnnotations;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Account
{
	public class LoginViewModel
	{
		public ScrollerVerifyViewModel Verify { get; set; }
		[Required]
		[StringLength(32, ErrorMessage = "非法的{0}", MinimumLength = 6)]
		public string UserName { get; set; }
		[Required]
		[StringLength(32, ErrorMessage = "非法的{0}", MinimumLength = 8)]
		public string Password { get; set; }
		public bool RememberMe { get; set; }
	}
}
