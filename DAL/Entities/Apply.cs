using System;
using System.Collections.Generic;
using System.Text;
using TrainSchdule.DAL.Entities;

namespace DAL.Entities
{
	public class Apply: BaseEntity
	{
		/// <summary>
		/// 申请人，考虑到申请人的信息可能会发生变化，此处应独立备份
		/// </summary>
		public virtual User From { get; set; }
		/// <summary>
		/// 申请发出人所在的单位
		/// </summary>
		public string Company { get; set; }
		/// <summary>
		/// 申请发出人家庭住址
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// 休假申请
		/// </summary>
		public virtual ApplyRequest Request { get; set; }
		/// <summary>
		/// 休假类别
		/// </summary>
		public string xjlb { get; set; }
		/// <summary>
		/// 附加记录
		/// </summary>
		public virtual ApplyStamp stamp { get; set; }

		/// <summary>
		/// 申请发布的时间
		/// </summary>
		public DateTime Create { get; set; }

		/// <summary>
		/// 提交到对应部门进行审核
		/// </summary>
		public virtual IEnumerable<Company> To { get; set; }

		/// <summary>
		/// 申请的审核情况
		/// </summary>
		public virtual IEnumerable<ApplyResponse> Response { get; set; }
		/// <summary>
		/// 申请的状态
		/// </summary>
		public AuditStatus Status { get; set; }

	}

	public enum AuditStatus
	{
		NotPublish=0,
		Withdrew=1,
		Auditing=2,
		AcceptAndWaitAdmin=4,
		Accept=8,
		Denied=16,
	}

}
