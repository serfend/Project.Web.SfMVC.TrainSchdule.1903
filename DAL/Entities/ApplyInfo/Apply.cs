using System;
using System.Collections.Generic;

namespace DAL.Entities.ApplyInfo
{
	public class Apply: BaseEntity
	{
		public virtual ApplyBaseInfo BaseInfo { get; set; }
		public virtual ApplyRequest RequestInfo { get; set; }
		public string AuditLeader { get; set; }
		/// <summary>
		/// 申请发布的时间
		/// </summary>
		public DateTime?Create { get; set; }

		/// <summary>
		/// 申请的审核情况
		/// </summary>
		public virtual IEnumerable<ApplyResponse> Response { get; set; }
		/// <summary>
		/// 终审单位
		/// </summary>
		public string FinnalAuditCompany { get; set; }
		/// <summary>
		/// 当前审批单位
		/// </summary>
		public string NowAuditCompany { get; set; }
		/// <summary>
		/// 当前审批单位名称
		/// </summary>
		public string NowAuditCompanyName { get; set; }
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
		NotSave=0,
		NotPublish=10,
		Withdrew=20,
		Denied=30,
		Auditing=40,
		AcceptAndWaitAdmin=50,
		Accept=100,
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
