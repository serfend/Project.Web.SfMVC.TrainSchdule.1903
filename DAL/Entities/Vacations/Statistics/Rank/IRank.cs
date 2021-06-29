using DAL.Entities.ZX.MemberRate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.Vacations.Statistics.Rank
{
    public interface IRank : IStatisticsBase, RoundCount, RankingResult
    {
        /// <summary>
        /// 申请类型
        /// </summary>
        string ApplyType { get; set; }
    }
}
