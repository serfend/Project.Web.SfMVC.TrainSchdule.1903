using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.UserInfo.UserAppMessage
{
    public class UserAppMessageInfo:BaseEntityGuid
    {
        public virtual User User { get; set; }
        public string UserId { get; set; }
        /// <summary>
        /// 用户设置
        /// </summary>
        public AppMessageSetting Setting { get; set; } = (int)AppMessageSetting.AllowAddByScan + AppMessageSetting.AllowStrangerMessage;
        /// <summary>
        /// 【冗余】粉丝数
        /// </summary>
        public int FansCount { get; set; }
        /// <summary>
        /// 【冗余】关注数
        /// </summary>
        public int FollowCount { get; set; }
        /// <summary>
        /// 【冗余】未读消息
        /// </summary>
        public int UnreadMessage { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
    [Flags]
    public enum AppMessageSetting
    {
        None=0,
        AllowStrangerMessage = 1,
        AllowAddByShareCard = 2,
        AllowAddByUserId = 4,
        AllowAddByScan = 8,
        AllowAddByGroup = 16,
        AllowAddByCompany = 32
    }
}
