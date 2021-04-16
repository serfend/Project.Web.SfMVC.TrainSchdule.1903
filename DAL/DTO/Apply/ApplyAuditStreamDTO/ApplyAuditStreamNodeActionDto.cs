using DAL.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.DTO.Apply.ApplyAuditStreamDTO
{
	public class ApplyAuditStreamNodeActionDto : MembersFilterDto
	{
		/// <summary>
		/// id
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// 审批节点的名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 说明
		/// </summary>
		public string Description { get; set; }

		public DateTime Create { get; set; }
		public string CompanyRegion { get; set; }
	}

	public class ApplyAuditStreamNodeActionVDto : MembersFilterVDto
	{
		/// <summary>
		/// id
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// 审批节点的名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 说明
		/// </summary>
		public string Description { get; set; }

		public DateTime Create { get; set; }
	}
}