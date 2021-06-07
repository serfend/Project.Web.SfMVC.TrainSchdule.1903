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
    }
    public enum PartyUserRecordType
    {
        None = 0,
        Host = 1,
        Voting = 2,
        Nonvoting = 4,
        Absent = 8
    }
}
