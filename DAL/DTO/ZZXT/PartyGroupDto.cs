using DAL.Entities.ZZXT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.ZZXT
{
    public class PartyGroupDto
    {
        public string Alias { get; set; }
        public DateTime Create { get; set; }
        public string Company { get; set; }
        public GroupType GroupType { get; set; }
    }
}
