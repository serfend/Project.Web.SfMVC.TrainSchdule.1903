using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Account
{
	/// <summary>
	/// 
	/// </summary>
	public class ModifyPermissionsViewModel: GoogleAuthViewModel
	{
		/// <summary>
		/// 指定id
		/// </summary>
		public string Id { get; set; }
		/// <summary>
		/// 新的权限
		/// </summary>
		public string NewPermission { get; set; }

	}
}
