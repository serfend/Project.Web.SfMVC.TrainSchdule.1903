using BLL.Helpers;
using DAL.Entities.ApplyInfo;
using DAL.Entities.Common.DataDictionary;
using DAL.Entities.Vacations;
using System.Collections.Generic;

namespace TrainSchdule.ViewModels.Apply
{
	/// <summary>
	///
	/// </summary>
	public class ApplyAuditStatusViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public ApplyAuditStatusDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class ApplyAuditStatusDataModel
	{
		/// <summary>
		/// 休假状态
		/// </summary>
		public Dictionary<int, AuditStatusMessage> List { get; set; }

		/// <summary>
		/// 用户操作
		/// </summary>
		public Dictionary<string, ActionByUserItem> Actions { get; set; }

		/// <summary>
		/// 假期类型表
		/// </summary>
		public Dictionary<string, VacationType> VacationTypes { get; set; }

		/// <summary>
		/// 休假落实情况
		/// </summary>
		public Dictionary<int, CommonDataDictionary> ExecuteStatus { get; set; }
	}
}