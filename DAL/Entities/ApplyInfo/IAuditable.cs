using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.ApplyInfo
{
	/// <summary>
	/// 资源可被审批
	/// </summary>
    public interface IAuditable
    {

		/// <summary>
		/// 本申请使用的审批流方案
		/// </summary>
		public  ApplyAuditStreamSolutionRule ApplyAuditStreamSolutionRule { get; set; }

		/// <summary>
		/// 本申请需要进行的步骤
		/// </summary>
		public  IEnumerable<ApplyAuditStep> ApplyAllAuditStep { get; set; }

		public string AuditLeader { get; set; }
		/// <summary>
		/// 当前审批步骤应有哪些人审批
		/// </summary>
		public  ApplyAuditStep NowAuditStep { get; set; }

		/// <summary>
		/// 申请的审核情况（此项用于展示当前有哪些人已经参与了审批）
		/// 并根据NowAuditStep决定是否需要添加更多的审批人
		/// </summary>
		public  IEnumerable<ApplyResponse> Response { get; set; }
		public AuditStatus Status { get; set; }

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
		Cancel = 120
	}

}
