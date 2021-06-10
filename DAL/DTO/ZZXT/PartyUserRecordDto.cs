using DAL.Entities;
using DAL.Entities.ZZXT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.ZZXT
{
    public class PartyUserRecordDto:BaseEntityGuid
    {

        public PartyUserRecordType Type { get; set; }
        public string UserId { get; set; }
        public Guid ConferenceId { get; set; }
        public DateTime Create { get; set; }
    }
}
