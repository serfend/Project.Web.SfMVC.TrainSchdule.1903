using DAL.Entities;
using DAL.Entities.ApplyInfo;
using System;
using DAL.DTO.User;

namespace DAL.DTO.Apply
{
	public class ApplySummaryDto : BaseEntity
	{
		public UserSummaryDto UserBase { get; set; }
		public ApplyBaseInfoDto Base { get; set; }
		public string NowAuditCompany { get; set; }
		public string VocationPlace { get; set; }
		public string HomePlace { get; set; }
		public DateTime?StampLeave { get; set; }
		public DateTime?StampReturn { get; set; }
		public DateTime?Create { get; set; }
		
		public AuditStatus Status { get; set; }

		/// <summary>
		/// 当前用户/单位是否正在可审核中状态
		/// </summary>
		public bool AuditAvailable { get; set; }

	}
}
