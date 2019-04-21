using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.Web.ViewModels;

namespace TrainSchdule.ViewModels.Apply
{
	public class ApplyAuditStatusViewModel:APIDataModel
	{
		public ApplyAuditStatusData Data { get; set; }
	}

	public class ApplyAuditStatusData
	{
		public Dictionary<int, string> List { get; set; }
	}
}
