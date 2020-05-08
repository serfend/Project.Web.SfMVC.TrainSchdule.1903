using BLL.Helpers;
using DAL.Entities.ApplyInfo;
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
		///
		/// </summary>
		public Dictionary<int, AuditStatusMessage> List { get; set; }

		/// <summary>
		/// 用户操作
		/// </summary>
		public Dictionary<string, ActionByUserItem> Actions { get; set; }
	}
}