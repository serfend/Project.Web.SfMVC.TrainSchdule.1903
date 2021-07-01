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
        /// 获取指定日期期数
        /// </summary>
        /// <param name="date"></param>
        /// <param name="ratingType"></param>
        /// <returns></returns>
        public static int RoundOfDateTime(this DateTime date, RatingType ratingType) => ratingType switch
        {
            RatingType.Once => 0,
            RatingType.Daily => date.Year * 1000 + date.DayOfYear,
            RatingType.Weekly => (date.Year - (date.DayOfYear < 7 ? 1 : 0)) * 100 + Cal.GetWeekOfYear(date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday),
            RatingType.Monthly => (date.Year) * 100 + date.Month - 1,
            RatingType.Seasonly => (date.Year) * 10 + (date.Month - 1) / 3,
            RatingType.HalfYearly => (date.Year) * 10 + (date.Month - 1) / 6,
            RatingType.Yearly => (date.Year),
            RatingType.All => 0,
            _ => 0
        };
        public static int RoundIndexToRound(this int roundIndex, RatingType ratingType)
        {
            var d = new DateTime(2000, 1, 1);
            var result = ratingType switch
            {
                RatingType.Once => 0,
                RatingType.Daily => 1000 * d.AddDays(roundIndex).Year + d.AddDays(roundIndex).DayOfYear,
                RatingType.Weekly => roundIndex > 1e5 ? 0 : Cal.AddWeeks(d, roundIndex).Year * 100 + Cal.GetWeekOfYear(Cal.AddWeeks(d, roundIndex), CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday),
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
                RatingType.Daily => round < 1e3 ? 0 : (int)new DateTime(round / 1000, 1, 1).AddDays(round % 1000).Subtract(d).TotalDays,
                RatingType.Weekly => round > 1e6 ? 0 : (int)Math.Ceiling((Cal.AddWeeks(d.AddYears(round / 100 - 2000), round % 100).Subtract(d).TotalDays / 7)),
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
        private static (DateTime, DateTime) DayAdd(int round, DateTime d)
        {
            if (round > 1e7 || round < 1e3) return (DateTime.MinValue, DateTime.MinValue);
            var t = new DateTime(round / 1000, 1, 1).AddDays(round % 1000 - 1);
            return (t, t.AddDays(1));
        }
        private static (DateTime, DateTime) WeekAdd(int round, DateTime d)
        {
            if (round > 1e6 || round < 1e2) return (DateTime.MinValue, DateTime.MinValue);
            var yearFirstDay = new DateTime(round / 100, 1, 1);
            var firstMonday = yearFirstDay.YearFirstWeekDay(DayOfWeek.Monday);
            var t = Cal.AddWeeks(firstMonday, round % 100);
            return (t, t.AddDays(7));
        }
        private static (DateTime, DateTime) MonthAdd(int round)
        {
            if (round > 1e6 || round < 1e2) return (DateTime.MinValue, DateTime.MinValue);
            var t = new DateTime(round / 100, 1, 1).AddMonths(round % 100);
            return (t, t.AddMonths(1));
        }
        private static (DateTime, DateTime) SeasonAdd(int round)
        {
            if (round > 1e5 || round < 10) return (DateTime.MinValue, DateTime.MinValue);
            var t = new DateTime(round / 10, 1, 1).AddMonths((round % 10) * 3);
            return (t, t.AddMonths(3));
        }
        private static (DateTime, DateTime) HalfyearAdd(int round)
        {
            if (round > 1e5 || round < 10) return (DateTime.MinValue, DateTime.MinValue);
            var t = new DateTime(round / 10, 1, 1).AddMonths((round % 10) * 6);
            return (t, t.AddMonths(6));
        }
        private static (DateTime, DateTime) YearAdd(int round)
        {
            if (round > 1e4 || round < 1) return (DateTime.MinValue, DateTime.MinValue);
            return (new DateTime(round, 1, 1), new DateTime(round + 1, 1, 1));
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
                RatingType.Daily => DayAdd(round, d),
                RatingType.Weekly => WeekAdd(round, d),
                RatingType.Monthly => MonthAdd(round),
                RatingType.Seasonly => SeasonAdd(round),
                RatingType.HalfYearly => HalfyearAdd(round),
                RatingType.Yearly => YearAdd(round),
                RatingType.All => (d, DateTime.MaxValue),
                _ => (DateTime.Now, DateTime.Now)
            };
        }
    }
}