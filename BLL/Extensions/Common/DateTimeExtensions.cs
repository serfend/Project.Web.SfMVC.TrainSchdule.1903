using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Extensions.Common
{
	public static class DateTimeExtensions
	{
		/// <summary>
		/// 统计结束时间不可晚于今日
		/// </summary>
		/// <param name="vEnd"></param>
		/// <returns></returns>
		public static DateTime EndDateNotEarlyThanNow(this DateTime vEnd, DateTime? now = null)
		{
			if (now == null) now = DateTime.Today.Date;
			return vEnd >= now.Value ? now.Value : vEnd;
		}

		public static DateTime LastMilliSecondInDay(this DateTime vEnd) => vEnd.AddDays(1).AddMilliseconds(-1);

		public static DateTime FirstYearOfTheDay(this DateTime vEnd) => new DateTime(vEnd.Year, 1, 1);
	}
}