using DAL.Entities.ApplyInfo;
using System.Collections.Generic;

namespace DAL.DTO.Apply
{
	public class ApplyAuditVdto<T> where T : IAuditable
	{
		public IEnumerable<ApplyAuditNodeVdto<T>> List { get; set; }
		public Entities.UserInfo.User AuditUser { get; set; }
	}

	public class ApplyAuditNodeVdto<T> where T: IAuditable
	{
		public AuditResult Action { get; set; }
		public string Remark { get; set; }
		public T AuditItem { get; set; }
	}

	public enum AuditResult
	{
		NoAction = 0,
		Accept = 1,
		Deny = 2
	}
}