using DAL.Entities;
using DAL.Entities.Game_r3;
using DAL.Entities.ZZXT;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.ZZXT
{
    public class UserPartyInfoDto : BaseEntityGuid
    {
        [Required(ErrorMessage = "用户名未填写")]
        public string UserName { get; set; }
        /// <summary>
        /// 党内职务
        /// </summary>
        [Required(ErrorMessage = "党内职务未填写")]
        public string Dilabo { get; set; }
        /// <summary>
        /// 起任时间
        /// </summary>
        [Required(ErrorMessage = "起任时间未填写")]
        public DateTime Appotime { get; set; }
        /// <summary>
        /// 部职别
        /// </summary>
        [Required(ErrorMessage = "部职别未填写")]
        public string Post { get; set; }
        /// <summary>
        /// 工作时间
        /// </summary>
        [Required(ErrorMessage = "工作时间未填写")]
        public DateTime Ruwutime { get; set; }
        /// <summary>
        /// 党团时间
        /// </summary>
        [Required(ErrorMessage = "党团时间未填写")]
        public DateTime Rudtime { get; set; }
        /// <summary>
        /// 政治面貌
        /// </summary>
        [Required(ErrorMessage = "政治面貌未填写")]
        public int Zzmm { get; set; }
        /// <summary>
        /// 党小组号
        /// </summary>
        [Required(ErrorMessage = "党小组号未填写")]
        public Guid DxzNum { get; set; }
        /// <summary>
        /// 转入时间
        /// </summary>
        [Required(ErrorMessage = "转入时间未填写")]
        public DateTime Posichan { get; set; }

        /// <summary>
        /// 单位代码
        /// </summary>
        [Required(ErrorMessage = "单位代码未填写")]
        public string Number { get; set; }

    }
}
