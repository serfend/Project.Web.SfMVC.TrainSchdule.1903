using DAL.Entities;
using DAL.Entities.Game_r3;
using DAL.Entities.ZZXT;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.ZZXT
{
    public class UserPartyInfoDto : BaseEntityGuid
    {
        public string UserName { get; set; }
        /// <summary>
        /// 党内职务
        /// </summary>
        public string Dilabo { get; set; }
        /// <summary>
        /// 起任时间
        /// </summary>
        public DateTime Appotime { get; set; }
        /// <summary>
        /// 部职别
        /// </summary>
        public string Post { get; set; }
        /// <summary>
        /// 工作时间
        /// </summary>
        public DateTime Ruwutime { get; set; }
        /// <summary>
        /// 党团时间
        /// </summary>
        public DateTime Rudtime { get; set; }
        /// <summary>
        /// 政治面貌
        /// </summary>
        public TypeInParty Zzmm { get; set; }
        /// <summary>
        /// 党小组号
        /// </summary>
        public Guid DxzNum { get; set; }
        /// <summary>
        /// 转入时间
        /// </summary>
        public DateTime Posichan { get; set; }
        /// <summary>
        /// 单位代码
        /// </summary>
        public string Number { get; set; }
    }
}
