using System;

namespace DAL.Entities.ApplyInfo
{
	/// <summary>
	/// 用户的申请请求
	/// </summary>
	public class ApplyRequest:BaseEntity
	{
		public DateTime StampLeave { get; set; } 
		public DateTime StampReturn { get; set; }
		public int OnTripLength { get; set; }
		public int VocationLength { get; set; }
		public string VocationType { get; set; }
		public virtual AdminDivision VocationPlace { get; set; }
		public string Reason { get; set; }
	}
}
