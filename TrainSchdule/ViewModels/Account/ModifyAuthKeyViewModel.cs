using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Account
{
	/// <summary>
	/// 
	/// </summary>
	public class ModifyAuthKeyViewModel:GoogleAuthViewModel
	{
		/// <summary>
		/// 
		/// </summary>
		public string NewKey { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string ModifyUserId { get; set; }
	}
}
