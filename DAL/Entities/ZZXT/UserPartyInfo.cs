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
        public int TypeInParty { get; set; }
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
}
