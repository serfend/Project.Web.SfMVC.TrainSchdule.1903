using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.ApplyInfo
{
	public class Apply : BaseEntity
	{
		public virtual ApplyBaseInfo BaseInfo { get; set; }
		public virtual ApplyRequest RequestInfo { get; set; }
		public string AuditLeader { get; set; }

		/// <summary>
		/// 申请发布的时间
		/// </summary>
		public DateTime? Create { get; set; }

		/// <summary>
		/// 申请的审核情况（此项用于展示当前有哪些人已经参与了审批）
		/// 并根据NowAuditStep决定是否需要添加更多的审批人
		/// </summary>
		public virtual IEnumerable<ApplyResponse> Response { get; set; }

		/// <summary>
		/// 本申请使用的审批流方案
		/// </summary>
		public virtual ApplyAuditStreamSolutionRule ApplyAuditStreamSolutionRule { get; set; }

		/// <summary>
		/// 本申请需要进行的步骤
		/// </summary>
		public virtual IEnumerable<ApplyAuditStep> ApplyAllAuditStep { get; set; }

		/// <summary>
		/// 当前审批步骤应有哪些人审批
		/// </summary>
		public virtual ApplyAuditStep NowAuditStep { get; set; }

		/// <summary>
		/// 申请的状态
		/// </summary>
		public AuditStatus Status { get; set; }

		/// <summary>
		/// 被召回的id
		/// </summary>
		public Guid? RecallId { get; set; }

		/// <summary>
		/// 申请是否可见（当用户尝试删除申请，且申请的状态为【已通过】时，将不删除，而改为隐藏
		/// </summary>
		public bool Hidden { get; set; }
	}

	public enum AuditStatus
	{
		NotSave = 0,
		NotPublish = 10,
		Withdrew = 20,
		Auditing = 40,
		AcceptAndWaitAdmin = 50,
		Denied = 75,
		Accept = 100,
	}

	public class AuditStatusMessage
	{
		public int Code { get; set; }
		public string Message { get; set; }
		public string Desc { get; set; }
		public string Color { get; set; }

		/// <summary>
		/// 可进行的操作
		/// </summary>
		public IEnumerable<string> Acessable { get; set; }
	}
}