using System.ComponentModel.DataAnnotations;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.User
{
	public class UserManageRangeModifyViewModel
	{
		[Required]
		public GoogleAuthViewModel Auth { get; set; }
		[Required]
		public string Code { get; set; }
		public string Id { get; set; }
	}
}
