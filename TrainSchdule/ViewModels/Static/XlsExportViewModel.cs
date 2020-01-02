using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Static
{
	/// <summary>
	/// 导出申请到指定模板
	/// </summary>
	public class AppliesExportDataModel
	{
		/// <summary>
		/// 模板名称
		/// </summary>
		public string Templete { get; set; }
		/// <summary>
		/// 导出申请的条件
		/// </summary>
		public QueryApplyDataModel Query { get; set; }
	}
}
