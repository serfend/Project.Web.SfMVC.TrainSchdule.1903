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

		private static DateTime start = DateTime.Today.AddYears(-1);
		private static DateTime end = DateTime.Today;

		public static DateTime Start { get => start; set => start = value; }
		public static DateTime End { get => end; set => end = value; }

		public YearlyVocationStatistics(ApplicationDbContext context) : base(context, Start, End, $"Year{Start.Year}")
		{
			_context = context;
		}

	}
}
