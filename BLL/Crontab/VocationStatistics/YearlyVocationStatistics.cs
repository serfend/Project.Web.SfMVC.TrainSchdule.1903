using DAL.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.Crontab
{
	/// <summary>
	/// 每年休假情况统计
	/// </summary>
	public class YearlyVocationStatistics : BaseOnTimeVocationStatistics
	{
		private readonly ApplicationDbContext _context;

		public static DateTime Start = DateTime.Today.AddYears(-1);
		public static DateTime End = DateTime.Today;
		public YearlyVocationStatistics(ApplicationDbContext context) : base(context, Start, End, $"{Start.Year}_{new GregorianCalendar().GetWeekOfYear(Start, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday) + 1}")
		{
			_context = context;
		}

	}
}
