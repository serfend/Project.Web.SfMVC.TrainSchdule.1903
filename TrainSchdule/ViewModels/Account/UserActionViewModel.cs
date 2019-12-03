using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Account
{
	/// <summary>
	/// 获取用户操作记录
	/// </summary>
	public class UserActionViewModel:ApiDataModel
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
