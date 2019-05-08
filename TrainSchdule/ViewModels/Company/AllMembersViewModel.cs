using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.DTO.User;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Company
{
	/// <summary>
	/// 单位全部成员
	/// </summary>
	public class AllMembersViewModel:ApiDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public AllMembersDataModel Data { get; set; }
	}

	/// <summary>
	/// 单位全部成员
	/// </summary>
	public class AllMembersDataModel
	{
		/// <summary>
		/// 用户列表
		/// </summary>
		public IEnumerable<UserSummaryDto> List { get; set; }
	}
}
