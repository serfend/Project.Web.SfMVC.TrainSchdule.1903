using DAL.Entities.Vacations.Statistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.Vacations
{
	public class StatisticsAppliesProcess : IStatisticsBase
	{
		/// <summary>
		/// 有效申请的数量
		/// </summary>
		public int ApplyCount { get; set; }

		/// <summary>
		/// 有效申请的天数（申请通过的天数-召回减少的天数）
		/// </summary>
		public int ApplySumDayCount { get; set; }

		public string Type { get; set; }
		public string CompanyCode { get; set; }
		public int Id { get; set; }
		public DateTime Target { get; set; }
	}
}