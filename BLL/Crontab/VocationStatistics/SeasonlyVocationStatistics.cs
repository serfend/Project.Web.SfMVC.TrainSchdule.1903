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
	public class SeasonlyVocationStatistics : BaseOnTimeVocationStatistics
	{
		public static DateTime Start = DateTime.Today.AddMonths(-3);
		public static DateTime End = DateTime.Today;
		public SeasonlyVocationStatistics(ApplicationDbContext context) : base(context, Start, End, $"{Start.Year}_Season{(int)(1 + ( Start.Month - 1) / 3d)}")
		{
		}
	}
}
