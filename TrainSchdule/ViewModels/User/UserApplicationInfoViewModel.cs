using System;
using DAL.Entities.UserInfo;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.User
{
	public class UserApplicationInfoViewModel:APIDataModel
	{
		public UserApplicationDataModel Data { get; set; }
	}

	public class UserApplicationDataModel
	{
		public string InvitedBy { get; set; }
		public string About { get; set; }
		public DateTime?Create { get; set; }
		public string Email { get; set; }
	}

	public static class UserApplicationInfoExtensions
	{
		public static UserApplicationDataModel ToModel(this UserApplicationInfo model)
		{
			return new UserApplicationDataModel()
			{
				About = model.About,
				Create = model.Create,
				Email = model.Email,
				InvitedBy = model.InvitedBy
			};
		}
	}
}
