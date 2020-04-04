using DAL.Data;
using System;

namespace TrainSchdule.Crontab
{
	/// <summary>
	/// 每月休假情况统计
	/// </summary>
	public class SeasonlyVocationStatistics : BaseOnTimeVocationStatistics
	{
		private static DateTime start = DateTime.Today.AddMonths(-3);
		private static DateTime end = DateTime.Today;

		public static DateTime Start { get => start; set => start = value; }
		public static DateTime End { get => end; set => end = value; }

		public SeasonlyVocationStatistics(ApplicationDbContext context) : base(context, Start, End, $"{Start.Year}_Season{(int)(1 + (Start.Month - 1) / 3d)}")
		{
		}
	}
}