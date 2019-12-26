using System.Collections.Generic;
using BLL.Helpers;
using DAL.Entities.ApplyInfo;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Apply
{
	/// <summary>
	/// 
	/// </summary>
	public class ApplyAuditStatusViewModel:ApiResult
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
		public  Dictionary<int, AuditStatusMessage> List { get; set; }
	}
}
