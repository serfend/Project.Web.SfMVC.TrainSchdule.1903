using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.DTO.Apply
{
	public class ApplyAuditVDTO
	{
		public AuditResult Action { get; set; }
		public string Remark { get; set; }
		public Entities.ApplyInfo.Apply Apply { get; set; }
		public Entities.UserInfo.User AuditUser { get; set; }
	}

	public enum AuditResult
	{
		NoAction=0,
		Accept=1,
		Deny=2
	}
}
