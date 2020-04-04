using BLL.Helpers;
using DAL.Entities.UserInfo;
using System.Collections.Generic;

namespace TrainSchdule.ViewModels.Account
{
	/// <summary>
	/// 获取用户操作记录
	/// </summary>
	public class UserActionViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public UserActionDataModel Data { get; set; }
	}

	/// <summary>
	/// 操作记录
	/// </summary>
	public class UserActionDataModel
	{
		/// <summary>
		/// 列表
		/// </summary>
		public IEnumerable<UserAction> List { get; set; }

		/// <summary>
		/// 总量
		/// </summary>
		public int TotalCount { get; set; }
	}
}