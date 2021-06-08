using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.ZZXT
{
    public class UserPartyInfo:BaseEntityGuid
    {
        [ForeignKey("UserName")]
        public virtual UserInfo.User User { get; set; }
        public string UserName { get; set; }
        /// <summary>
        /// 党内职务
        /// </summary>
        public virtual PartyDuty Duty { get; set; }
        public int DutyId { get; set; }
        /// <summary>
        /// 起任时间
        /// </summary>
        public DateTime DutyStart { get; set; }
        /// <summary>
        /// 政治面貌
        /// </summary>
        public TypeInParty TypeInParty { get; set; }
        /// <summary>
        /// 单位代码 冗余PartyGroup.Company
        /// </summary>
        public virtual Company Company { get; set; }
        public string CompanyCode { get; set; }
        /// <summary>
        /// 党小组
        /// </summary>
        public virtual PartyGroup PartyGroup { get; set; }
        public Guid PartyGroupId { get; set; }
        /// <summary>
        /// 转入时间
        /// </summary>
        public DateTime Create { get; set; }
    }
    public enum TypeInParty
    {
        /// <summary>
        /// 无
        /// </summary>
        None=0,
        /// <summary>
        /// 群众
        /// </summary>
        [Description("群众")]
        Masses = 1,
        /// <summary>
        /// 少先队员
        /// </summary>
        [Description("少先队员")]
        MemberL1 = 2,
        /// <summary>
        /// 团员
        /// </summary>
        [Description("团员")]
        MemberL2 = 4,
        /// <summary>
        /// 入党积极分子
        /// </summary>
        [Description("入党积极分子")]
        MemberL3V1 = 8,
        /// <summary>
        /// 预备党员
        /// </summary>
        [Description("预备党员")]
        MemberL3V2 = 10,
        /// <summary>
        /// 党员
        /// </summary>
        [Description("党员")]
        MemberL3V3 = 12
    }
}
