using DAL.DTO.User;
using System.Collections.Generic;

namespace DAL.DTO.Company
{
	public class CompanyDto
	{
		public string Code { get; set; }
		public string Name { get; set; }
		/// <summary>
		/// 单位类型 连/科 组/部 部 首长
		/// </summary>
		public string CompanyTypeDesc { get; set; }
		public string CompanyParentTypeDesc { get; set; }
		public IEnumerable<UserSummaryDto> Managers { get; set; }
	}
}
