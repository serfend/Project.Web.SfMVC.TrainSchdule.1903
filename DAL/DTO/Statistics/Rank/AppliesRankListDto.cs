using DAL.Entities.Vacations.Statistics.Rank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Statistics.Rank
{

    /// <summary>
    /// 适用于排行榜展示
    /// </summary>
    public class AppliesRankListDto
    {
        public IEnumerable<RankItem> List { get; set; }
        public int TotalCount { get; set; }
        public RankItem Self { get; set; }
    }
    public class RankItem
    {
        public string User { get; set; }
        public string RealName { get; set; }
        /// <summary>
        /// 状态 检查是否在申请进行中
        /// </summary>
        public string Status { get; set; }
        public int Rank { get; set; }
        public int LastRank { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public int Level { get; set; }
    }
}
