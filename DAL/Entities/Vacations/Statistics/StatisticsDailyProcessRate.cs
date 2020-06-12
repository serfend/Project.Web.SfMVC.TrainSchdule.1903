using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.Vacations.Statistics
{
	/// <summary>
	/// 统计各类比率和人数等需要按天累计项
	/// </summary>
	public class StatisticsDailyProcessRate : IStatisticsBase
	{
		/// <summary>
		/// 单位本级人员数量<see cref="单位人员总数"/>
		/// </summary>
		public int MembersCount { get; set; }

		/// <summary>
		/// 已完成全年休假指标的人数<see cref="休满假数（率）"/>
		/// </summary>
		public int CompleteYearlyVacationCount { get; set; }

		/// <summary>
		/// 休假率低于60%人员数<see cref="休假率低于60%数"/>
		/// </summary>
		public int MembersVacationDayLessThanP60 { get; set; }

		/// <summary>
		/// 有效申请的人数
		/// </summary>
		public int ApplyMembersCount { get; set; }

		/// <summary>
		/// 应当休假总天数<see cref="应（实）休总天数"/>
		/// </summary>
		public int CompleteVacationExpectDayCount { get; set; }

		/// <summary>
		/// 实际休假总天数<see cref="应（实）休总天数"/>
		/// </summary>
		public int CompleteVacationRealDayCount { get; set; }

		public string Type { get; set; }
		public string CompanyCode { get; set; }
		public int Id { get; set; }
		public DateTime Target { get; set; }
	}
}