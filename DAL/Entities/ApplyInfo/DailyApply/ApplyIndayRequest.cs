using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.ApplyInfo.DailyApply
{
    /// <summary>
    /// 日请假请求
    /// </summary>
    public class ApplyIndayRequest:BaseEntityGuid, IApplyRequestBase
    {
        public DateTime? StampLeave { get; set; }
        public DateTime? StampReturn { get; set; }
        public virtual AdminDivision VacationPlace { get ; set ; }
        public int VacationPlaceCode { get; set; }
        public string VacationPlaceName { get ; set ; }
        public string Reason { get ; set ; }
        /// <summary>
        /// 请假类别
        /// </summary>
        public string RequestType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        public Transportation ByTransportation { get; set; }

    }
}
