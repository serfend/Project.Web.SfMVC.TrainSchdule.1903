using DAL.Entities.UserInfo;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
	/// <summary>
	/// 单位主管决定单位的审批需要经过哪些人
	/// </summary>
	public class CompanyManagers : BaseEntityGuid
	{
		[ForeignKey("UserId")]
		public virtual User User { get; set; }
		public string UserId { get; set; }
		[ForeignKey("CompanyCode")]
		public virtual Company Company { get; set; }
		public string CompanyCode { get; set; }
		public virtual User AuthBy { get; set; }
		public DateTime? Create { get; set; }
	}
}