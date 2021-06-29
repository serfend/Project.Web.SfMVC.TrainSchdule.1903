using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.Vacations.Statistics.Rank
{
    public class StatisticsApplyRankRecord: StatisticsApplyRank
    {
        /// <summary>
        /// 上次更新时间
        /// </summary>
        public DateTime LastUpdate { get; set; }
        /// <summary>
        /// 根据时间判断是否已经完成最终成绩确定
        /// </summary>
        public bool FinnalResult { get; set; }
    }
}
