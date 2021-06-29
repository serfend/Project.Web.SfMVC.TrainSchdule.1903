using DAL.Entities.UserInfo;
using DAL.Entities.ZX.MemberRate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.Vacations.Statistics.Rank
{
    public class StatisticsApplyRankItem: StatisticsApplyRank { 
        /// <summary>
        /// 用户状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 上次排名
        /// </summary>
        public int LastRank { get; set; }
        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string UserRealName { get; set; }
    }
    public class StatisticsApplyRank : IRank
    {
        public int Id { get; set; }
        public string ApplyType { get; set; }
        public string Type { get; set; }
        public string CompanyCode { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public DateTime Target { get; set; }
        public int RatingCycleCount { get; set; }
        public RatingType RatingType { get; set; }
        public int Rank { get; set; }
        public int Level { get; set; }
    }
}
