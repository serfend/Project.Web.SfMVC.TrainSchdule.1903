using BLL.Helpers;
using DAL.Entities.UserInfo;
using System;
using System.Collections;
using System.Collections.Generic;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	/// 
	/// </summary>
	public class UserBaseInfoViewModel : ApiResult
	{
		/// <summary>
		/// 用户基本信息
		/// </summary>
		public UserBaseInfo Data { get; set; }
	}
	public class UserBaseInfoWithIdViewModel:ApiResult
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
	public class UsersBaseInfoWithIdDataModel
	{
		public IEnumerable<UserBaseInfoWithIdDataModel> List { get; set; }
	}
	/// <summary>
	/// 用户id列表
	/// </summary>
	public class UsersBaseInfoWithIdViewModel:ApiResult
	{
		public UsersBaseInfoWithIdDataModel Data { get; set; }
	}
}
