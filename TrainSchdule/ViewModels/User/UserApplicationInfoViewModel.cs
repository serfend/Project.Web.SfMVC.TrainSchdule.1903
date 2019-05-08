using System;
using DAL.Entities.UserInfo;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	/// 
	/// </summary>
	public class UserApplicationInfoViewModel:ApiDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public UserApplicationDataModel Data { get; set; }
	}
	/// <summary>
	/// 
	/// </summary>
	public class UserApplicationDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public string InvitedBy { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string About { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime?Create { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Email { get; set; }
	}
	/// <summary>
	/// 
	/// </summary>
	public static class UserApplicationInfoExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
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
