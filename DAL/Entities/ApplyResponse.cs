using System;
using System.Collections.Generic;
using System.Text;
using TrainSchdule.DAL.Entities.UserInfo;

namespace DAL.Entities
{
	public class ApplyResponse:BaseEntity
	{
		/// <summary>
		/// 处理人
		/// </summary>
		public virtual User AuditingBy { get; set; }
		/// <summary>
		/// 处理审核的单位
		/// </summary>
		public virtual Company Company { get; set; }
		/// <summary>
		/// 审核状态
		/// </summary>
		public Auditing Status { get; set; }
		/// <summary>
		/// 单位处理审核的时间
		/// </summary>
		public DateTime HandleStamp { get; set; }
		/// <summary>
		/// 单位批复
		/// </summary>
		public string Remark { get; set; }

	}
	public enum Auditing {
		UnReceive=0,
		Received=1,
		Accept=4,
		Denied=8
	}
}
