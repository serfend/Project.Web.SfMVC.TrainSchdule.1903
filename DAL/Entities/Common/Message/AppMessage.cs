using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.Common.Message
{
    /// <summary>
    /// 站内消息
    /// </summary>
   public class AppMessage:BaseEntityGuid
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Create { get; set; }
        /// <summary>
        /// 消息来源用户
        /// </summary>
        public virtual User From { get; set;}
        public string FromId { get; set; }
        /// <summary>
        /// 消息目标用户
        /// </summary>
        public virtual User To { get; set; }
        public string ToId { get; set; }
        /// <summary>
        /// 消息状态
        /// </summary>
        public AppMessageStatus Status { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public virtual AppMessageContent Content { get; set; }

        /// <summary>
        /// 【冗余】消息长度
        /// </summary>
        public int Length { get; set; }
    }
    [Flags]
    public enum AppMessageStatus
    {
        None=0,
        Read = 1,
        Handled=2,
        Recall=4,
        DeletedByFrom = 8,
        DeletedByTo = 16,
        IsSystem=32,// 系统消息应无发出方
    }
}
