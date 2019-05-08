using DAL.Entities.UserInfo;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	/// 
	/// </summary>
	public class UserBaseInfoViewModel:ApiDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public UserBaseInfoDataModel Data { get; set; }
	}
	/// <summary>
	/// 
	/// </summary>
	public class UserBaseInfoDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public string Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string RealName { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Avatar { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public GenderEnum Gender { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public bool PrivateAccount { get; set; }
	}

	/// <summary>
	/// 
	/// </summary>
	public static class UserBaseInfoExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <param name="userid"></param>
		/// <returns></returns>
		public static UserBaseInfoDataModel ToModel(this UserBaseInfo model,string userid)
		{
			return  new UserBaseInfoDataModel()
			{
				Id = userid,
				Avatar = model.Avatar ?? (model.Gender == GenderEnum.Female ? @"\images\defaults\def-female-logo.png" : @"\images\defaults\def-male-logo.png"),
				Gender = model.Gender,
				PrivateAccount = model.PrivateAccount,
				RealName = model.RealName
			};
		}
	}
}
