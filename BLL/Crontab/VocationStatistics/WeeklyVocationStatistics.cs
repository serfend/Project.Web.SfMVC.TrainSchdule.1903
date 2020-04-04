using DAL.Data;
using System;
using System.Globalization;

namespace TrainSchdule.Crontab
{
	/// <summary>
	/// 每周休假情况统计
	/// </summary>
	public class WeeklyVocationStatistics : BaseOnTimeVocationStatistics
	{
		private readonly ApplicationDbContext _context;

		private static DateTime start = DateTime.Today.AddDays(-1);
		private static DateTime end = DateTime.Today;

		public static DateTime Start { get => start; set => start = value; }
		public static DateTime End { get => end; set => end = value; }

		public WeeklyVocationStatistics(ApplicationDbContext context) : base(context, Start, End, $"{Start.Year}_Week{new GregorianCalendar().GetWeekOfYear(Start, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday) + 1}")
		{
			_context = context;
		}
	}
}