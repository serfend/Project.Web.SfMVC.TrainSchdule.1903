using DAL.Entities.ApplyInfo;
using System;

namespace DAL.DTO.Apply
{
	public class ApplyResponseDto
	{
		public  string AuditingUserRealName { get; set; }
		public string CompanyName { get; set; }
		public Auditing Status { get; set; }
		public DateTime? HandleStamp { get; set; }
		public string Remark { get; set; }
	}
}
