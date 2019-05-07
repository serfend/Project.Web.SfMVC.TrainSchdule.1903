using System;
using System.Collections.Generic;
using System.Text;
using DAL.DTO.User;

namespace DAL.DTO.Company
{
	public class CompanyDTO
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public IEnumerable<UserSummaryDTO> Managers { get; set; }
	}
}
