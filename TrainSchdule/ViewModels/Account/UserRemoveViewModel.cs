using DAL.Entities.UserInfo;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Account
{
	/// <summary>
	/// 
	/// </summary>
	public class UserRemoveViewModel: IdSubmitViewModel
	{
		
	}
	public class UserApplicationViewModel : IdSubmitViewModel
	{
		public ApplicationUser Data { get; set; }
	}
}
