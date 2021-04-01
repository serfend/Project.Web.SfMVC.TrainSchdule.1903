﻿using DAL.Entities;
using DAL.Entities.ApplyInfo;
using System;
using DAL.DTO.User;
using System.Collections.Generic;
using DAL.DTO.Apply.ApplyAuditStreamDTO;

namespace DAL.DTO.Apply
{
	public class ApplySummaryDto<T> : BaseEntityGuid where T : IApplyRequestBase
	{
		/// <summary>
		/// 用户基本信息
		/// </summary>
		public UserSummaryDto UserBase { get; set; }

		/// <summary>
		/// 申请基本信息
		/// </summary>
		public ApplyBaseInfoDto Base { get; set; }

		public T Request { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? Create { get; set; }

		/// <summary>
		/// 审批状态
		/// </summary>

		public AuditStatus Status { get; set; }

		/// <summary>
		/// 假期主状态，根据人员情况联动
		/// </summary>
		public MainStatus MainStatus { get; set; }
		/// <summary>
		/// 休假落实情况
		/// </summary>
		public ExecuteStatus ExecuteStatus { get; set; }

		public Guid? ExecuteStatusId { get; set; }

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