using DAL.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.DTO.Apply.ApplyAuditStreamDTO
{
	public class ApplyAuditStreamDto
	{
		/// <summary>
		/// id
		/// </summary>
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		/// <summary>
		/// 审批流全流节点,以##分割表示多个
		/// </summary>
		public IEnumerable<string> Nodes { get; set; }

		public DateTime Create { get; set; }
		public string CompanyRegion { get; set; }
	}

	public class ApplyAuditStreamVDto : IRegion
	{
		/// <summary>
		/// id
		/// </summary>
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		/// <summary>
		/// 审批流全流节点
		/// </summary>
		public IEnumerable<ApplyAuditStreamNodeActionVDto> Nodes { get; set; }

		public DateTime Create { get; set; }
		public string RegionOnCompany { get; set; }
	}
}