using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Account
{
	/// <summary>
	/// 
	/// </summary>
	public class ModifyAuthKeyViewModel
	{
		/// <summary>
		/// 
		/// </summary>
		public GoogleAuthViewModel Auth { get; set; }
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
