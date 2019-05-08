using DAL.DTO.User;
using System.Collections.Generic;

namespace DAL.DTO.Company
{
	public class CompanyDto
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public IEnumerable<UserSummaryDto> Managers { get; set; }
	}
}
