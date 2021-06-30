using Abp.Linq.Expressions;
using BLL.Extensions;
using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.IVacationStatistics.Rank;
using DAL.Data;
using DAL.DTO.Statistics.Rank;
using DAL.Entities.ApplyInfo;
using DAL.Entities.ApplyInfo.DailyApply;
using DAL.Entities.UserInfo;
using DAL.Entities.Vacations;
using DAL.Entities.Vacations.Statistics.Rank;
using DAL.Entities.ZX.MemberRate;
using DAL.QueryModel;
using Hangfire;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.VacationStatistics.Rank
{
    public partial class StatisticsApplyRankServices
    {
        public IEnumerable<StatisticsApplyRankItem> QueryRankList(QueryRankListViewModel model, out int totalCount)
        {
            var list = context.StatisticsApplyRanks.AsQueryable();
            if (model.User != null)
                list = list.Where(c => c.UserId == model.User.Value);
            if (model.EntityType != null)
                list = list.Where(c => c.ApplyType == model.EntityType.Value);
            if (model.Company != null)
                list = list.Where(c => c.CompanyCode == model.Company.Value);
            if (model.Date == null) model.Date = new QueryByDate() { Start = DateTime.Now };
            if (model.RankType == null || model.RankType.Start == null) throw new ActionStatusMessageException(new ApiResult(ActionStatusMessage.StaticMessage.IdIsNull, "排序类型未指定", true));
            var round = model.Date.Start.RoundOfDateTime((RatingType)model.RankType.Start);
            list = list.Where(c => c.RatingCycleCount == round).Where(c => (int)c.RatingType == model.RankType.Start);
            var result = list.OrderBy(c => c.Rank).SplitPage(model.Page);
            totalCount = result.Item2;
            return result.Item1;
        }

    }
    public partial class StatisticsApplyRankServices
    {
        public void SaveResultList(List<StatisticsApplyRankItem> list)
        {
            Dictionary<string, Tuple<string, string>> userStatusCache = new Dictionary<string, Tuple<string, string>>();
            var ua = userActionServices.Log(UserOperation.FromSystemReport, null, $"处理排行榜统计数据:{list.Count}", false);
            Func<StatisticsApplyRankItem, Expression<Func<StatisticsApplyRankItem, bool>>> expBuilder = record =>
            {
                var lastRound = record.RatingCycleCount.NextRound(record.RatingType, -1);
                Expression<Func<StatisticsApplyRankItem, bool>> expression = i => i.ApplyType == record.ApplyType
               && i.CompanyCode == record.CompanyCode
               && i.UserId == record.UserId
               && i.RatingType == record.RatingType
               && i.RatingCycleCount == lastRound;
                return expression;
            };
            for (var i = 0; i < list.Count; i++)
            {
                if (i % 1e3 == 0)
                    userActionServices.Status(ua, true, JsonConvert.SerializeObject(new { c = i, t = DateTime.Now }));
                var record = list[i];
                var exp = expBuilder(record);
                var last = list.Where(exp.Compile()).FirstOrDefault();
                if (last == null) last = context.StatisticsApplyRanks.Where(exp).FirstOrDefault();
                record.LastRank = last?.Rank ?? -1;
                if (record.UserId == null) record.UserId = record.User?.Id;
                if (record.UserId == null) continue;
                if (!userStatusCache.ContainsKey(record.UserId))
                {
                    var item = new Tuple<string, string>(record.User.GetUserStatus(context), record.User.BaseInfo.RealName);
                    userStatusCache[record.UserId] = item;
                }
                var recordItem = userStatusCache[record.UserId];
                record.Status = recordItem.Item1;
                record.UserRealName = recordItem.Item2;
            }
            userActionServices.Status(ua, true, $"完成:{DateTime.Now}");
            context.StatisticsApplyRanks.AddRange(list);
        }
        public void ReloadRange(DateTime start, DateTime end)
        {
            userActionServices.Log(UserOperation.FromSystemReport, null, "重新加载排行榜", true);
            ReloadIndayRange(start, end);
            ReloadVacationRange(start, end);
            context.SaveChanges();
        }

        public void Reload()
        {
            userActionServices.Log(UserOperation.FromSystemReport, null, $"加载排行榜:[日更新]", true);
            ReloadVacationRank(DateTime.Now);
            ReloadIndayRank(DateTime.Now);
            context.SaveChanges();
        }
    }
    public partial class StatisticsApplyRankServices : IStatisticsApplyRankServices
    {
        private readonly ApplicationDbContext context;
        private readonly IUserActionServices userActionServices;
        private readonly List<Tuple<string, IQueryable<Apply>>> vacationTypes;
        private readonly List<Tuple<string, IQueryable<ApplyInday>>> indayTypes;


        public StatisticsApplyRankServices(ApplicationDbContext context, IUserActionServices userActionServices)
        {
            this.context = context;
            this.userActionServices = userActionServices;
            this.vacationTypes = context.VacationTypes
                .Where(a => !a.Disabled)
                .Select(a => a.Name).ToList().Select(i => new Tuple<string, IQueryable<Apply>>(
                    i, context.AppliesDb
               .Where(a => a.Status == AuditStatus.Accept)
               .Where(a => a.RequestInfo.VacationType == i)
                    )).ToList(); ;
            this.indayTypes = context.VacationIndayTypes
                .Where(a => !a.Disabled)
                .Select(a => a.Name).ToList().Select(i => new Tuple<string, IQueryable<ApplyInday>>(
                    i, context.AppliesIndayDb
               .Where(a => a.Status == AuditStatus.Accept)
               .Where(a => a.RequestInfo.RequestType == i)
                    )).ToList();
        }
    }
}
