using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.ZZXT
{
    public class PartyDuty : BaseEntityInt
    {
        public string Name { get; set; }
        public int Priority { get; set; }
        public bool Enable { get; set; }
    }
}
