using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.Crontab
{
	/// <summary>
	/// 每月休假情况统计
	/// </summary>
	public class MonthlyVocationStatstics : BaseOnTimeVocationStatistics
	{
		public static DateTime Start = DateTime.Today.AddMonths(-1);
		public static DateTime End = DateTime.Today;
		public MonthlyVocationStatstics(ApplicationDbContext context) : base(context, Start, End,$"{Start.Year}_Month{Start.Month}")
		{
		}
	}
}
