using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.Common.Message
{
    /// <summary>
    /// 用户产生的关系
    /// </summary>
    public class AppUserRelate:BaseEntityGuid
    {
        public virtual User From { get; set; }
        public string FromId { get; set; }
        public virtual User To { get; set; }
        public string ToId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Create { get; set; }
        /// <summary>
        /// 关系状态
        /// </summary>
        public Relation Relation { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }

    }
    [Flags]
    public enum Relation
    {
        None = 0,
        Follow = 1,
        Block = 2,
        //BlockActive TODO 不允许看动态
    }
}
