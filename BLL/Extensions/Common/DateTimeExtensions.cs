using DAL.Entities.ZX.MemberRate;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BLL.Extensions.Common
{
    public static class DateTimeExtensions
    {
        public static GregorianCalendar Cal = new GregorianCalendar();
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
        /// <summary>
        /// 获取指定日期相对于2000-1-1的期数
        /// </summary>
        /// <param name="date"></param>
        /// <param name="ratingType"></param>
        /// <returns></returns>
        public static int RoundOfDateTime(this DateTime date, RatingType ratingType)
        {
            var d = new DateTime(2000, 1, 1);
            var result = ratingType switch
            {
                RatingType.Once => 0,
                RatingType.Daily => (int)Math.Ceiling(date.Subtract(d).TotalDays),
                RatingType.Weekly => (date.Year - d.Year - (date.DayOfYear < 7 ? 1 : 0)) * 100 + Cal.GetWeekOfYear(date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday),
                RatingType.Monthly => (date.Year - d.Year) * 100 + date.Month,
                RatingType.Seasonly => (date.Year - d.Year) * 10 + date.Month / 3,
                RatingType.HalfYearly => (date.Year - d.Year) * 10 + date.Month / 6,
                RatingType.Yearly => (date.Year - d.Year),
                RatingType.All => 0,
                _ => 0
            };
            return result;
        }
        public static int RoundIndexToRound(this int roundIndex, RatingType ratingType)
        {
            var d = new DateTime(2000, 1, 1);
            var result = ratingType switch
            {
                RatingType.Once => 0,
                RatingType.Daily => roundIndex,
                RatingType.Weekly => (Cal.AddWeeks(d, roundIndex).Year - d.Year) * 100 + Cal.GetWeekOfYear(Cal.AddWeeks(d, roundIndex), CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday),
                RatingType.Monthly => 100 * (roundIndex / 12) + roundIndex % 12,
                RatingType.Seasonly => 10 * (roundIndex / 4) + roundIndex % 4,
                RatingType.HalfYearly => 10 * (roundIndex / 2) + roundIndex % 2,
                RatingType.Yearly => roundIndex,
                RatingType.All => 0,
                _ => 0
            };
            return result;
        }
        public static int RoundToRoundIndex(this int round, RatingType ratingType)
        {
            var d = new DateTime(2000, 1, 1);
            var result = ratingType switch
            {
                RatingType.Once => 0,
                RatingType.Daily => round,
                RatingType.Weekly => (int)Math.Ceiling((Cal.AddWeeks(d.AddYears(round / 100), round % 100).Subtract(d).TotalDays / 7)),
                RatingType.Monthly => 12 * (round / 100) + round % 100,
                RatingType.Seasonly => 4 * (round / 10) + round % 10,
                RatingType.HalfYearly => 2 * (round / 10) + round % 2,
                RatingType.Yearly => round,
                RatingType.All => 0,
                _ => 0
            };
            return result;
        }
        public static int NextRound(this int round, RatingType ratingType, int value = 1) => (round.RoundToRoundIndex(ratingType) + value).RoundIndexToRound(ratingType);
        //public static int NextRound(this int round, RatingType ratingType, int value = 1)
        //{
        //    var d = new DateTime(2000, 1, 1);
        //    var result = ratingType switch
        //    {
        //        RatingType.Once => 0,
        //        RatingType.Daily => round + 1,
        //        RatingType.Weekly => Cal.AddWeeks(d.AddYears(round / 100), round % 100).Year > d.AddYears(round / 100).Year ? (100 * (round / 100 + 1)) : (round + 1),
        //        RatingType.Monthly => round % 100 == 12 ? (100 * (round / 100 + 1)) : (round + 1),
        //        RatingType.Seasonly => round % 10 == 4 ? (10 * (round / 10 + 1)) : (round + 1),
        //        RatingType.HalfYearly => round % 10 == 2 ? (10 * (round / 10 + 1)) : (round + 1),
        //        RatingType.Yearly => round + 1,
        //        RatingType.All => 0,
        //        _ => 0
        //    };
        //    return result;
        //}
        public static DateTime YearFirstWeekDay(this DateTime date, DayOfWeek dayOfWeek)
        {
            var firstMonday = date.AddDays(-(int)Cal.GetDayOfWeek(date)).AddDays((int)dayOfWeek);
            return firstMonday;
        }
        private static (DateTime, DateTime) WeekAdd(int round, DateTime d)
        {
            var yearFirstDay = d.AddYears(round / 100);
            var firstMonday = yearFirstDay.YearFirstWeekDay(DayOfWeek.Monday);
            var t = Cal.AddWeeks(firstMonday, round % 100);
            return (t, t.AddDays(7));
        }
        private static (DateTime, DateTime) MonthAdd(int round, DateTime d)
        {
            var t = d.AddYears(round / 100).AddMonths(round % 100 - 1);
            return (t, t.AddMonths(1));
        }
        private static (DateTime, DateTime) SeasonAdd(int round, DateTime d)
        {
            var t = d.AddYears(round / 10).AddMonths((round % 10 - 1) * 3);
            return (t, t.AddMonths(3));
        }
        private static (DateTime, DateTime) HalfyearAdd(int round, DateTime d)
        {
            var t = d.AddYears(round / 10).AddMonths((round % 10 - 1) * 6);
            return (t, t.AddMonths(6));
        }
        /// <summary>
        /// 获取指定期数从2000-1-1开始的日期范围
        /// </summary>
        /// <param name="round"></param>
        /// <param name="ratingType"></param>
        /// <returns></returns>
        public static (DateTime, DateTime) DateTimeRangeOfRound(this int round, RatingType ratingType)
        {
            var d = new DateTime(2000, 1, 1);
            return ratingType switch
            {
                RatingType.Daily => (d.AddDays(round), d.AddDays(round + 1)),
                RatingType.Weekly => WeekAdd(round, d),
                RatingType.Monthly => MonthAdd(round, d),
                RatingType.Seasonly => SeasonAdd(round, d),
                RatingType.HalfYearly => HalfyearAdd(round, d),
                RatingType.Yearly => (d.AddYears(round), d.AddYears(round + 1)),
                RatingType.All => (d, DateTime.MaxValue),
                _ => (DateTime.Now, DateTime.Now)
            };
        }
    }
}