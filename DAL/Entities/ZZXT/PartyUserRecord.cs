using DAL.Entities.ZZXT.Conference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.ZZXT
{
    /// <summary>
    /// 人员操作记录
    /// </summary>
    public class PartyUserRecord : BaseEntityGuid
    {
        public PartyUserRecordType Type { get; set; }
        public virtual UserInfo.User User { get; set; }
        public string UserId { get; set; }
        public virtual PartyBaseConference Conference { get; set; }
        public Guid ConferenceId { get; set; }
        public DateTime Create { get; set; }
    }
    public class PartyUserRecordContent : BaseEntityGuid
    {
        /// <summary>
        /// 参会记录
        /// </summary>
        public virtual PartyUserRecord Record { get; set; }
        public Guid RecordId { get; set; }
        /// <summary>
        /// 会议记录类型
        /// 使用PartyConferRecordType获取字典对应类型
        /// </summary>
        public int ContentType { get; set; }
        public string Content { get; set; }
        public DateTime Create { get; set; }
    }
    public enum PartyUserRecordType
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 主持
        /// </summary>
        Host = 1,
        /// <summary>
        /// 参会
        /// </summary>
        Voting = 2,
        /// <summary>
        /// 列席
        /// </summary>
        Nonvoting = 4,
        /// <summary>
        /// 督导
        /// </summary>
        Monitor = 8,
        /// <summary>
        /// 审核
        /// </summary>
        Audit = 16,
        /// <summary>
        /// 负责
        /// </summary>
        Duty = 32,
        /// <summary>
        /// 监制
        /// </summary>
        Producer = 64,
        /// <summary>
        /// 发布
        /// </summary>
        Publish = 128,
        /// <summary>
        /// 缺席
        /// </summary>
        Absent = 256
    }
}
