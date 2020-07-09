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
	public class ApplyRequest : BaseEntityGuid
	{
		public DateTime? StampLeave { get; set; }
		public DateTime? StampReturn { get; set; }

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
		/// 休假地点
		/// </summary>
		public virtual AdminDivision VacationPlace { get; set; }

		/// <summary>
		/// 休假地点（详细地址）
		/// </summary>
		public string VacationPlaceName { get; set; }

		/// <summary>
		/// 休假原因（用户设置）
		/// </summary>
		public string Reason { get; set; }

		/// <summary>
		/// 创建时用户的全年休假情况
		/// </summary>
		public string VacationDescription { get; set; }

		public DateTime CreateTime { get; set; }
		public Transportation ByTransportation { get; set; }
	}

	public enum Transportation
	{
		其他 = -1,
		火车 = 0,
		飞机 = 1,
		汽车 = 2,
	}
}