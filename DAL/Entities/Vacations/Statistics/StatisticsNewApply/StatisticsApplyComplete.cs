using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.Vacations.Statistics.StatisticsNewApply
{
	/// <summary>
	/// 完成休假申请的统计
	/// </summary>
	public class StatisticsApplyComplete : IStatisticsApplyBase
	{
		public string CompanyCode { get; set; }
		public byte To { get; set; }
		public byte From { get; set; }
		public string Type { get; set; }
		public DateTime Target { get; set; }
		public int Id { get; set; }
		public int Value { get; set; }
		public int Day { get; set; }
	}
}