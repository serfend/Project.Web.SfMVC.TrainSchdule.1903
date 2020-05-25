using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entities.ApplyInfo
{
	/// <summary>
	/// 当前审批的步骤执行情况信息
	/// </summary>
	public class ApplyAuditStep : BaseEntity
	{
		/// <summary>
		/// 步骤在全流程中所处位置
		/// </summary>
		public int Index { get; set; }

		/// <summary>
		/// 可进行审批的成员列表,以##分割
		/// </summary>
		public string MembersFitToAudit { get; set; }

		/// <summary>
		/// 审批的成员有哪些已通过审批，以##分割
		/// </summary>
		public string MembersAcceptToAudit { get; set; }

		/// <summary>
		/// 需要有多少人通过审批
		/// </summary>
		public int RequireMembersAcceptCount { get; set; }

		/// <summary>
		/// 当前步骤对应的Node名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 为了便于观察，展示首个审批人的单位
		/// </summary>
		public string FirstMemberCompanyName { get; set; }

		/// <summary>
		/// 为了便于观察，展示首个审批人的单位
		/// </summary>
		public string FirstMemberCompanyCode { get; set; }
	}
}