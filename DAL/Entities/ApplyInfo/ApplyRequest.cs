using DAL.Entities.Vacations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.ApplyInfo
{
	/// <summary>
	/// 用户的申请请求
	/// </summary>
	public class ApplyRequest : BaseEntityGuid, IApplyRequestBase
	{
		/// <summary>
		/// 路途长度
		/// </summary>
		public int OnTripLength { get; set; }

		/// <summary>
		/// 主假期长度
		/// </summary>
		public int VacationLength { get; set; }

		/// <summary>
		/// 休假类别
		/// </summary>
		public string VacationType { get; set; }

		/// <summary>
		/// 福利假，包含法定节假日自动计算
		/// </summary>
		public virtual IEnumerable<VacationAdditional> AdditialVacations { get; set; }


		/// <summary>
		/// 创建时用户的全年休假情况
		/// </summary>
		public string VacationDescription { get; set; }

		public DateTime CreateTime { get; set; }
		public Transportation ByTransportation { get; set; }
        public DateTime? StampLeave { get ; set ; }
        public DateTime? StampReturn { get ; set ; }
        public virtual AdminDivision VacationPlace { get ; set ; }
		public int VacationPlaceCode { get; set; }
        public string VacationPlaceName { get ; set ; }
        public string Reason { get ; set ; }
    }

	public enum Transportation
	{
		未知 = -1,
		火车 = 0,
		飞机 = 1,
		汽车 = 2,
		出租车 = 3,
		私家车 = 6,
		地铁=8,
		步行=12,
		其他=15
	}
}