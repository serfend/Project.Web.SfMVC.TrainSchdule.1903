using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.ZZXT
{
    public class PartyConferRecordContentDto:BaseEntityGuid
    {
        public Guid RecordId { get; set; }
        /// <summary>
        /// 会议记录类型
        /// 使用PartyConferRecordType获取字典对应类型
        /// </summary>
        public int ContentType { get; set; }
        public string Content { get; set; }
        public DateTime Create { get; set; }
    }
}
