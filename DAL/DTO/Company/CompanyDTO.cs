using DAL.DTO.User;
using DAL.Entities;
using System.Collections.Generic;

namespace DAL.DTO.Company
{
	public class CompanyDto
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		/// <summary>
		/// 单位情况<see cref="CompanyStatus"/>
		/// </summary>
		public CompanyStatus CompanyStatus { get; set; }

		/// <summary>
		/// 单位所在位置
		/// </summary>
		public AdminDivision Location { get; set; }

		public IEnumerable<string> Tags { get; set; }
		public IEnumerable<UserSummaryDto> Managers { get; set; }
	}
}