using BLL.Helpers;
using DAL.Entities.UserInfo;
using System.Collections.Generic;

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

	/// <summary>
	///
	/// </summary>
	public class UserBaseInfoWithIdViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public UserBaseInfoWithIdDataModel Data { get; set; }
	}

	/// <summary>
	/// 用户基本信息增加用户id
	/// </summary>
	public class UserBaseInfoWithIdDataModel
	{
		/// <summary>
		///
		/// </summary>
		public UserBaseInfo Base { get; set; }

		/// <summary>
		///
		/// </summary>
		public string Id { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UsersBaseInfoWithIdDataModel
	{
		/// <summary>
		///
		/// </summary>
		public IEnumerable<UserBaseInfoWithIdDataModel> List { get; set; }
	}

	/// <summary>
	/// 用户id列表
	/// </summary>
	public class UsersBaseInfoWithIdViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public UsersBaseInfoWithIdDataModel Data { get; set; }
	}
}