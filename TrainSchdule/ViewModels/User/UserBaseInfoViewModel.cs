using DAL.Entities.UserInfo;
using System;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	/// 
	/// </summary>
	public class UserBaseInfoViewModel : ApiDataModel
	{
		/// <summary>
		/// 用户基本信息
		/// </summary>
		public UserBaseInfo Data { get; set; }
	}
	public class UserBaseInfoWithIdViewModel:ApiDataModel
	{
		public UserBaseInfoWithIdDataModel Data { get; set; }
	}
	/// <summary>
	/// 用户基本信息增加用户id
	/// </summary>
	public class UserBaseInfoWithIdDataModel 
	{
		public UserBaseInfo Base { get; set; }
		public string Id { get; set; }
	}

}
