using DAL.Entities;
using DAL.Entities.ApplyInfo;
using System;
using DAL.DTO.User;

namespace DAL.DTO.Apply
{
	public class ApplySummaryDto : BaseEntity
	{
		/// <summary>
		/// 用户基本信息
		/// </summary>
		public UserSummaryDto UserBase { get; set; }
		/// <summary>
		/// 申请基本信息
		/// </summary>
		public ApplyBaseInfoDto Base { get; set; }
		/// <summary>
		/// 当前审批单位
		/// </summary>
		public string NowAuditCompany { get; set; }
		public ApplyRequest Request { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime?Create { get; set; }
		/// <summary>
		/// 审批状态
		/// </summary>
		
		public AuditStatus Status { get; set; }

		/// <summary>
		/// 当前用户/单位是否正在可审核中状态
		/// </summary>
		public bool AuditAvailable { get; set; }

	}
}
