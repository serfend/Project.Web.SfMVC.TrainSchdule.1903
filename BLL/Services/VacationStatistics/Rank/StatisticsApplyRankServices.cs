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
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        /// <summary>
        /// 算法写的太龊了，希望后人给改改
        /// 为了实现效率高，列表中 applyType, ratingCycleCount, ratingType三个字段须一致
        /// </summary>
        /// <param name="list"></param>
        public void SaveResultList(List<StatisticsApplyRankItem> list)
        {
            if (list.Count == 0) return;
            var ua = userActionServices.Log(UserOperation.FromSystemReport, null, $"处理排行榜统计数据:{list.Count}", list.Count == 0);
            int no_user_id_count = 0;
            int step = (int)1e3;
            for (var i = 0; i < list.Count; i++)
            {
                if (i % step == 0)
                {
                    userActionServices.Status(ua, true, JsonConvert.SerializeObject(new { c = i, t = DateTime.Now }));
                    if (i > 0) context.StatisticsApplyRanks.AddRange(list.Skip(i - step).Take(step));
                    context.SaveChanges();
                }
                var record = list[i];
                if (record.UserId == null) record.UserId = record.User?.Id;
                if (record.UserId == null)
                {
                    no_user_id_count++;
                    continue;
                }
                var usrStatusKey = $"com:user:apply:status:{record.UserId}";
                var exist = redisCacheClient.Db7.ExistsAsync(usrStatusKey).Result;
                if (!exist)
                {
                    var item = new Tuple<string, string, string>(record.User.GetUserStatus(context), record.User.BaseInfo.RealName, record.User.CompanyInfo.Company?.Name);
                    redisCacheClient.Db7.AddAsync(usrStatusKey, item, TimeSpan.FromMinutes(30)).Wait();
                }
                var recordItem = redisCacheClient.Db7.GetAsync<Tuple<string, string, string>>(usrStatusKey).Result;
                record.Status = recordItem.Item1;
                record.UserRealName = recordItem.Item2;
                record.UserCompany = recordItem.Item3;
                var rankKey = $"com:sts:apply:rank:{record.UserId}:{record.ApplyType}.{record.CompanyCode}.{record.RatingType}.";
                var rankKeyLast = $"{rankKey}{record.RatingCycleCount.NextRound(record.RatingType, -1)}";
                exist = redisCacheClient.Db7.ExistsAsync(rankKeyLast).Result;
                record.LastRank = exist ? redisCacheClient.Db7.GetAsync<int>(rankKeyLast).Result : -1;
                redisCacheClient.Db7.AddAsync($"{rankKey}{record.RatingCycleCount}", record.Rank,TimeSpan.FromDays(7)).Wait();
            }
            context.StatisticsApplyRanks.AddRange(list.Skip(step * (list.Count / step)).Take(list.Count % step));
            userActionServices.Status(ua, true, $"完成:{DateTime.Now},{nameof(no_user_id_count)}{no_user_id_count}");
            context.SaveChanges();
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
        private readonly IRedisCacheClient redisCacheClient;
        private readonly List<Tuple<string, IQueryable<Apply>>> vacationTypes;
        private readonly List<Tuple<string, IQueryable<ApplyInday>>> indayTypes;
        public int RankCount = 50;

        public StatisticsApplyRankServices(ApplicationDbContext context, IConfiguration configuration, IUserActionServices userActionServices, IRedisCacheClient redisCacheClient)
        {

            this.context = context;
            var cfg_RankCount = configuration?.GetSection("Configuration")?.GetSection("App")?.GetSection("Apply")?.GetValue<int>("RankCount") ?? RankCount;
            RankCount = cfg_RankCount > 0 ? cfg_RankCount : RankCount;
            this.userActionServices = userActionServices;
            this.redisCacheClient = redisCacheClient;
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
