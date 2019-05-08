using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities.ApplyInfo;

namespace DAL.DTO.Apply
{
	public class ApplyResponseDTO
	{
		public  string AuditingUserRealName { get; set; }
		public string CompanyName { get; set; }
		public Auditing Status { get; set; }
		public DateTime? HandleStamp { get; set; }
		public string Remark { get; set; }
	}
}
