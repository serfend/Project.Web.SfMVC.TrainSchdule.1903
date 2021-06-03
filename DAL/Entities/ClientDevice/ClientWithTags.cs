using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.ClientDevice
{
    public class ClientWithTags:BaseEntityGuid
    {
        public virtual Client Client { get; set; }
        public Guid ClientId { get; set; }
        public virtual ClientTags ClientTags { get; set; }
        public Guid ClientTagsId { get; set; }
    }
}
