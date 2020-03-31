using BLL.Helpers;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Account
{
	/// <summary>
	///
	/// </summary>
	public class UserActionReportViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public UserActionDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserActionReportDataModel
	{
		/// <summary>
		/// 报告的账号
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// 报告的信息
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		///Debug = 32,
		///Infomation = 16,
		///Warning = 8,
		///Danger = 4,
		///Disaster = 0
		/// </summary>
		public ActionRank Rank { get; set; }
	}
}