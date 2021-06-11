using DAL.Entities.ClientDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
namespace DAL.Entities.ZZXT.Conference
{
    /// <summary>
    /// 通用会议
    /// </summary>
    public class PartyBaseConference:BaseEntityGuid
    {
        public string Title { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// 会议类型 使用<see cref="ApplicationDbContext.PartyConferType"/>
        /// </summary>
        public int Type { get; set; }
        public virtual Company CreateBy { get; set; }
        public string CreateByCode { get; set; }
        public DateTime Create { get; set; }
    }
    /// <summary>
    /// 会议的标签
    /// </summary>
    public  class PartyConferWithTag : BaseEntityGuid
    {
        public virtual PartyBaseConference Confer { get; set; }
        public Guid ConferId { get; set; }
        public virtual ClientTags ClientTags { get; set; }
        public Guid ClientTagsId { get; set; }
    }

}
