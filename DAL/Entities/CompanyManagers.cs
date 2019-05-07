using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities.UserInfo;

namespace DAL.Entities
{
	/// <summary>
	/// 单位主管决定单位的审批需要经过哪些人
	/// </summary>
	public class CompanyManagers:BaseEntity
	{
		public virtual User User { get; set; }
		public virtual Company Company { get; set; }
		public virtual User AuthBy { get; set; }
		public DateTime Create { get; set; }
	}
}
