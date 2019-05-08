using System.Collections.Generic;
using DAL.Entities.ApplyInfo;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Apply
{
	public class ApplyAuditStatusViewModel:APIDataModel
	{
		public ApplyAuditStatusDataModel Data { get; set; }
	}

	public class ApplyAuditStatusDataModel
	{
		public  Dictionary<int, AuditStatusMessage> List { get; set; }
	}
}
