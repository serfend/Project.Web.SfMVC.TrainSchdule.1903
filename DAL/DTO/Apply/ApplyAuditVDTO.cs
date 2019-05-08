namespace DAL.DTO.Apply
{
	public class ApplyAuditVdto
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
