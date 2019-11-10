using DAL.Entities.UserInfo;
using System;
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
		/// 工作/入伍时间
		/// </summary>
		public DateTime Time_Work { get; set; }
		/// <summary>
		/// 出生日期
		/// </summary>
		public DateTime Time_BirthDay { get; set; }
		/// <summary>
		/// 党团时间		
		/// </summary>
		public DateTime Time_Party { get; set; }
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
