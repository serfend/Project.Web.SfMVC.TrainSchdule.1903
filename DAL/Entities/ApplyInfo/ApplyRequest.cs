using DAL.Entities.Vocations;
using System;
using System.Collections.Generic;

namespace DAL.Entities.ApplyInfo
{
	/// <summary>
	/// 用户的申请请求
	/// </summary>
	public class ApplyRequest : BaseEntity
	{
		public DateTime? StampLeave { get; set; }
		public DateTime? StampReturn { get; set; }
		/// <summary>
		/// 路途长度
		/// </summary>
		public int OnTripLength { get; set; }
		/// <summary>
		/// 正休长度
		/// </summary>
		public int VocationLength { get; set; }
		/// <summary>
		/// 休假类别
		/// </summary>
		public string VocationType { get; set; }
		/// <summary>
		/// 福利假，包含法定节假日自动计算
		/// </summary>
		public virtual IEnumerable<VocationAdditional> AdditialVocations { get; set; }
		
		public virtual AdminDivision VocationPlace { get; set; }
		public string Reason { get; set; }
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
