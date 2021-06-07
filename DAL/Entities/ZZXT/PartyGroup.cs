using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.ZZXT
{
    /// <summary>
    /// 党団小组
    /// </summary>
    public class PartyGroup:BaseEntityGuid
    {
        public string Alias { get; set; }
        public DateTime Create { get; set; }
        public virtual Company Company { get; set; }
        public string CompanyCode { get; set; }
        public GroupType GroupType { get; set; }
    }
    public enum GroupType
    {
        None  = 0,
        /// <summary>
        /// 党
        /// </summary>
        Party = 1,
        /// <summary>
        /// 団
        /// </summary>
        League = 2
    }
}
