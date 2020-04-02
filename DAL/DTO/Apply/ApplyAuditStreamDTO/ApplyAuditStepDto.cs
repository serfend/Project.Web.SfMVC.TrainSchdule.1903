using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.DTO.Apply.ApplyAuditStreamDTO
{
	public class ApplyAuditStepDto : BaseEntity
	{
		/// <summary>
		/// 步骤在全流程中所处位置
		/// </summary>
		public int Index { get; set; }

		/// <summary>
		/// 可进行审批的成员列表,以##分割
		/// </summary>
		public IEnumerable<string> MembersFitToAudit { get; set; }

		/// <summary>
		/// 审批的成员有哪些已通过审批，以##分割
		/// </summary>
		public IEnumerable<string> MembersAcceptToAudit { get; set; }

		/// <summary>
		/// 需要有多少人通过审批
		/// </summary>
		public int RequireMembersAcceptCount { get; set; }
	}
}