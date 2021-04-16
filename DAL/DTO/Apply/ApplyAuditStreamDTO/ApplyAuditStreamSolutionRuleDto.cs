using DAL.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.DTO.Apply.ApplyAuditStreamDTO
{
	public class ApplyAuditStreamSolutionRuleDto : MembersFilterDto
	{
		/// <summary>
		/// id
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// 规则名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 规则描述
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 规则的优先级
		/// </summary>
		public int Priority { get; set; }

		/// <summary>
		/// 规则是否启用
		/// </summary>
		public bool Enable { get; set; }
		public string SolutionName { get; set; }

		public DateTime Create { get; set; }
		public string CompanyRegion { get; set; }
	}

	public class ApplyAuditStreamSolutionRuleVDto : MembersFilterVDto
	{
		/// <summary>
		/// id
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// 规则名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 规则描述
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 规则的优先级
		/// </summary>
		public int Priority { get; set; }

		/// <summary>
		/// 规则是否启用
		/// </summary>
		public bool Enable { get; set; }

		public string SolutionName { get; set; }

		public DateTime Create { get; set; }
	}
}