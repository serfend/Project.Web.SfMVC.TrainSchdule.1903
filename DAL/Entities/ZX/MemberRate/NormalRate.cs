using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.ZX.MemberRate
{
    /// <summary>
    /// 周期
    /// </summary>
    public interface RoundCount
    {

        /// <summary>
        /// 周期数:当前评分模式下距离 Date(0) 
        /// </summary>
        int RatingCycleCount { get; set; }
        /// <summary>
        /// 评分模式
        /// </summary>
        RatingType RatingType { get; set; }
    }
    /// <summary>
    /// 成绩
    /// </summary>
    public interface RankingResult
    {
        /// <summary>
        /// 排名
        /// </summary>
        int Rank { get; set; }
        /// <summary>
        /// 成绩
        /// </summary>
        int Level { get; set; }
    }
    /// <summary>
    /// 单位内排序
    /// </summary>
    public class NormalRate : BaseEntityGuid, RoundCount, RankingResult
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Create { get; set; }
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
        [ForeignKey("CompanyCode")]
        public virtual Company Company { get; set; }
        [ForeignKey("CompanyCode")]
        public string CompanyCode { get; set; }
        /// <summary>
        /// 参评人
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }

        /// <summary>
        /// 周期数:当前评分模式下距离 Date(0) 
        /// </summary>
        public int RatingCycleCount { get; set; }
        /// <summary>
        /// 评分模式
        /// </summary>
        public RatingType RatingType { get; set; }
    }
    /// <summary>
    /// 评分模式
    /// </summary>
    public enum RatingType
    {
        [Display(Name = "仅一次", AutoGenerateField = false)]
        Once = 0,
        [Display(Name = "按日", ShortName = "date")]
        Daily = 1,
        [Display(Name = "按周", ShortName = "week")]
        Weekly = 2,
        [Display(Name = "按月", ShortName = "month")]
        Monthly = 4,
        [Display(Name = "按季度", ShortName = "season")]
        Seasonly = 8,
        [Display(Name = "按半年", ShortName = "half-year")]
        HalfYearly = 12,
        [Display(Name = "按年度", ShortName = "year")]
        Yearly = 16,
        [Display(Name = "有史以来", ShortName = null)]
        All = 32
    }
}
