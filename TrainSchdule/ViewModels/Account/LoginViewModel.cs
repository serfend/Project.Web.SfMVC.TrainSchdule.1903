using System.ComponentModel.DataAnnotations;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Account
{
	/// <summary>
	/// 
	/// </summary>
	public class LoginViewModel
	{
		/// <summary>
		/// 
		/// </summary>
		public ScrollerVerifyViewModel Verify { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[Required]
		[StringLength(32, ErrorMessage = "非法的{0}", MinimumLength = 6)]
		public string UserName { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[Required]
		[StringLength(32, ErrorMessage = "非法的{0}", MinimumLength = 8)]
		public string Password { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public bool RememberMe { get; set; }
	}
}
