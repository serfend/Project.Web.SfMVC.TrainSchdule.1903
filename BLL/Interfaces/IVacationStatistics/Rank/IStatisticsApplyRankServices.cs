using DAL.Entities.Vacations.Statistics.Rank;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.IVacationStatistics.Rank
{
    /// <summary>
    /// 申请频度用户排行榜
    /// </summary>
    public interface IStatisticsApplyRankServices
    {
        /// <summary>
        /// 重新加载指定范围
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        void ReloadRange(DateTime start, DateTime end);
        /// <summary>
        /// 重新加载
        /// </summary>
        void Reload();
        /// <summary>
        /// 重新加载假期排名
        /// </summary>
        /// <param name="date"></param>
        List<StatisticsApplyRankItem> ReloadVacationRank(DateTime date);
        /// <summary>
        /// 重新请假排名
        /// </summary>
        /// <param name="date"></param>
        List<StatisticsApplyRankItem> ReloadIndayRank(DateTime date);
        /// <summary>
        /// 查询排名
        /// </summary>
        /// <returns></returns>
        IEnumerable<StatisticsApplyRankItem> QueryRankList(QueryRankListViewModel model,out int totalCount);
    }
}
