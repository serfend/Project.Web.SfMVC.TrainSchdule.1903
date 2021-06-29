using Abp.Linq.Expressions;
using BLL.Extensions;
using BLL.Extensions.Common;
using DAL.Entities.ApplyInfo.DailyApply;
using DAL.Entities.UserInfo;
using DAL.Entities.Vacations.Statistics.Rank;
using DAL.Entities.ZX.MemberRate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.VacationStatistics.Rank
{

    public partial class StatisticsApplyRankServices
    {
        private Func<ApplyInday, double> funcSumLength = f => (f.RequestInfo.StampReturn.Value - f.RequestInfo.StampLeave.Value).TotalSeconds;
        public void ReloadIndayRange(DateTime start, DateTime end)
        {
            var list = new List<StatisticsApplyRankItem>();
            ReloadIndayRankWithType((type, ratingType, db) =>
            {
                userActionServices.Log(UserOperation.FromSystemReport, null, $"加载排行榜:[{type}.{ratingType}]", true);
                var startRound = start.RoundOfDateTime(ratingType);
                var endRound = end.RoundOfDateTime(ratingType);
                for (var i = startRound; i < endRound; i = i.NextRound(ratingType))
                {
                    list.AddRange(ReloadIndayRank(type, i, true, ratingType, db));
                }
                list.AddRange(ReloadIndayRank(type, endRound, false, ratingType, db));
            });
            SaveResultList(list);
        }
        public void ReloadIndayRankWithType(Action<string, RatingType, IQueryable<ApplyInday>> reloadMethod)
        {
            var types = new List<RatingType>() { RatingType.Daily, RatingType.Weekly, RatingType.Monthly, RatingType.Seasonly, RatingType.HalfYearly, RatingType.Yearly, RatingType.All };
            foreach (var ratingType in types)
                foreach (var t in indayTypes)
                {
                    var applies = t.Item2;
                    var entityType = $"inday.{t.Item1}";
                    reloadMethod.Invoke(entityType, ratingType, applies);
                }
        }
        public List<StatisticsApplyRankItem> ReloadIndayRank(DateTime date)
        {
            var list = new List<StatisticsApplyRankItem>();
            if (date == DateTime.MinValue) date = DateTime.Now;
            ReloadIndayRankWithType((type, ratingType, db) =>
            {
                list.AddRange(ReloadIndayRank(type, date, ratingType, db));
            });
            SaveResultList(list);
            return list;
        }
        public List<StatisticsApplyRankItem> ReloadIndayRank(string entityType, DateTime start, RatingType type, IQueryable<ApplyInday> db)
        {
            // -1hour:当前阶段结束后，统计前一阶段全部内容
            int round = start.AddHours(-1).RoundOfDateTime(type);
            var isFinal = start.RoundOfDateTime(type) > round;
            return ReloadIndayRank(entityType, round, isFinal, type, db);
        }
        public List<StatisticsApplyRankItem> ReloadIndayRank(string entityType, int round, bool isFinal, RatingType type, IQueryable<ApplyInday> db)
        {
            var result = new List<StatisticsApplyRankItem>();
            var record = context.StatisticsApplyRankRecords
                .Where(a => a.RatingType == type)
                .Where(a => a.RatingCycleCount == round)
                .Where(a => a.ApplyType == entityType)
                .FirstOrDefault();
            if (record != null && record.FinnalResult) return result;
            if (record == null)
            {
                record = new StatisticsApplyRankRecord()
                {
                    ApplyType = entityType,
                    RatingCycleCount = round,
                    RatingType = type,
                };
                context.StatisticsApplyRankRecords.Add(record);
            }
            record.FinnalResult = isFinal;
            record.LastUpdate = DateTime.Now;
            // 移除上次统计
            var last_record = context.StatisticsApplyRanks
                .Where(a => a.ApplyType == $"{entityType}@c" || a.ApplyType == $"{entityType}@l")
                .Where(a => a.RatingCycleCount == round)
                .Where(a => a.RatingType == type);
            context.StatisticsApplyRanks.RemoveRange(last_record);

            // 开始本次统计
            var range = round.DateTimeRangeOfRound(type);
            var list = db.AsQueryable();
            list = list.Where(a => a.RequestInfo.StampLeave >= range.Item1);
            if (range.Item2 < DateTime.MaxValue) list = list.Where(a => a.RequestInfo.StampLeave <= range.Item2);
            var listStatistics = list.ToList();
            // 按次数和长度排序
            var group = listStatistics.GroupBy(a => a.BaseInfo.From, (a, b) => new
            {
                u = a,
                l = (int)b.Sum(funcSumLength),
                c = b.Count()
            });
            var rankByCount = group.OrderByDescending(a => a.c).ToList();
            var rankByLength = group.OrderByDescending(a => a.l).ToList();

            // 按单位排序
            var allCompanies = context.CompaniesDb.Select(c => c.Code).Distinct().ToList();
            allCompanies.Add(null);
            foreach (var c in allCompanies)
            {
                var usersByCount = rankByCount.Where(a => a.u.CompanyInfo.CompanyCode != null);
                var usersByLength = rankByLength.Where(a => a.u.CompanyInfo.CompanyCode != null);
                if (c != null)
                {
                    usersByCount = usersByCount.Where(a => a.u.CompanyInfo.CompanyCode.StartsWith(c));
                    usersByLength = usersByLength.Where(a => a.u.CompanyInfo.CompanyCode.StartsWith(c));
                }
                int index = 1;
                var entityTypeCount = $"{entityType}@c";
                foreach (var u in usersByCount)
                    result.Add(new StatisticsApplyRankItem()
                    {
                        ApplyType = entityTypeCount,
                        CompanyCode = c,
                        Level = u.c,
                        Rank = index++,
                        RatingCycleCount = round,
                        RatingType = type,
                        User = u.u,
                    });
                index = 1;
                var entityTypeLength = $"{entityType}@l";
                foreach (var u in usersByLength)
                    result.Add(new StatisticsApplyRankItem()
                    {
                        ApplyType = entityTypeLength,
                        CompanyCode = c,
                        Level = u.l,
                        Rank = index++,
                        RatingCycleCount = round,
                        RatingType = type,
                        User = u.u
                    });
            }
            return result;
        }
    }
}
