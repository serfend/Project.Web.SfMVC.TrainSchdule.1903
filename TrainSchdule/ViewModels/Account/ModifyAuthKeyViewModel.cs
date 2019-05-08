using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Account
{
	public class ModifyAuthKeyViewModel
	{
		public GoogleAuthViewModel Auth { get; set; }
		public string NewKey { get; set; }
		public string ModifyUserId { get; set; }
	}
}
