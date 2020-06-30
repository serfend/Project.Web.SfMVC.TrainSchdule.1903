using DAL.Entities;
using DAL.Entities.ApplyInfo;
using System;
using DAL.DTO.User;
using System.Collections.Generic;
using DAL.DTO.Apply.ApplyAuditStreamDTO;

namespace DAL.DTO.Apply
{
	public class ApplySummaryDto : BaseEntityGuid
	{
		/// <summary>
		/// 用户基本信息
		/// </summary>
		public UserSummaryDto UserBase { get; set; }

		/// <summary>
		/// 申请基本信息
		/// </summary>
		public ApplyBaseInfoDto Base { get; set; }

		public ApplyRequest Request { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? Create { get; set; }

		/// <summary>
		/// 审批状态
		/// </summary>

		public AuditStatus Status { get; set; }

		/// <summary>
		/// 召回id
		/// </summary>
		public Guid? RecallId { get; set; }

		/// <summary>
		/// 全流程
		/// </summary>
		public IEnumerable<ApplyAuditStepDto> Steps { get; set; }

		/// <summary>
		/// 当前流程
		/// </summary>
		public ApplyAuditStepDto NowStep { get; set; }

		/// <summary>
		/// 使用的Solution名称
		/// </summary>
		public string AuditStreamSolution { get; set; }
	}
}