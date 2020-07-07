using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using DAL.DTO.User;
using DAL.DTO.Apply.ApplyAuditStreamDTO;
using DAL.DTO.Recall;

namespace DAL.DTO.Apply
{
	public sealed class ApplyDetailDto : BaseEntityGuid
	{
		/// <summary>
		/// 批准首长
		/// </summary>
		public string AuditLeader { get; set; }

		/// <summary>
		/// 从用户原始库中导出的信息
		/// </summary>
		public UserSummaryDto Base { get; set; }

		/// <summary>
		/// 用户填写申请时的单位信息
		/// </summary>
		public Entities.Company Company { get; set; }

		/// <summary>
		/// 用户填写申请时的职务信息
		/// </summary>
		public Duties Duties { get; set; }

		/// <summary>
		/// 用户填写申请时的社会情况信息
		/// </summary>
		public UserSocialInfo Social { get; set; }

		/// <summary>
		/// 用户的休假请求
		/// </summary>
		public ApplyRequest RequestInfo { get; set; }

		public DateTime? Create { get; set; }
		public IEnumerable<ApplyResponseDto> Response { get; set; }
		public IEnumerable<ApplyAuditStepDto> Steps { get; set; }
		public ApplyAuditStepDto NowStep { get; set; }
		public string AuditSolution { get; set; }

		public AuditStatus Status { get; set; }

		/// <summary>
		/// 休假落实情况
		/// </summary>
		public ExecuteStatus ExecuteStatus { get; set; }

		public Guid? RecallId { get; set; }
		public Guid? ExecuteStatusId { get; set; }
		public bool Hidden { get; set; }

		/// <summary>
		/// 用户全年假期描述
		/// </summary>
		public UserVacationInfoVDto UserVacationDescription { get; set; }
	}
}