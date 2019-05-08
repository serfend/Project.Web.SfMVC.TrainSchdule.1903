using System.ComponentModel.DataAnnotations;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	/// 
	/// </summary>
	public class UserManageRangeModifyViewModel
	{
		/// <summary>
		/// 
		/// </summary>
		[Required]
		public GoogleAuthViewModel Auth { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[Required]
		public string Code { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Id { get; set; }
	}
}
