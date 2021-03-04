using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.ZX.MemberRate
{
    /// <summary>
    /// 单位内排序
    /// </summary>
    public class NormalRate:BaseEntityGuid
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Create { get; set; }
        /// <summary>
        /// 周期数:当前评分模式下距离 Date(0) 
        /// </summary>
        public int RatingCycleCount { get; set; }
        /// <summary>
        /// 评分模式
        /// </summary>
        public RatingType RatingType { get; set; }
        /// <summary>
        /// 排名
        /// </summary>
        public int Rank { get; set; }
        /// <summary>
        /// 分数 0 - 1000，可映射到等级 0-200不称职 201-400较差 401-600称职 601-800良好 801-1000优秀
        /// </summary>
        public int Level { get; set; }  
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 评比单位，默认为用户所在单位
        /// </summary>
        public virtual Company Company { get; set; }
        /// <summary>
        /// 参评人
        /// </summary>
        public virtual User User { get; set; }
    }
    /// <summary>
    /// 评分模式
    /// </summary>
    public enum RatingType
    {
        Once=0,
        Daily=1,
        Weekly=2,
        Monthly=4,
        Seasonly=8,
        Yearly=16,
    }
}
