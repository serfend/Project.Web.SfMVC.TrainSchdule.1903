using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.Vacations.Statistics.StatisticsNewApply
{
	public class StatisticsApplyBase : IDateStatistics
	{
		/// <summary>
		/// 单位代码
		/// </summary>
		public string CompanyCode { get; set; }

		public DateTime Target { get; set; }

		/// <summary>
		/// 休假目的地 前两位代码（表示省）
		/// </summary>
		public byte To { get; set; }

		/// <summary>
		/// 休假出发地 前两位代码（表示省）
		/// </summary>
		public byte From { get; set; }

		/// <summary>
		/// 用户类型
		/// </summary>
		public string Type { get; set; }
	}
}