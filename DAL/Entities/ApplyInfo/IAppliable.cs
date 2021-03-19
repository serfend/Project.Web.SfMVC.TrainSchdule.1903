using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.ApplyInfo
{
	/// <summary>
	/// 申请基本
	/// </summary>
    public interface IAppliable: IAuditable
	{
		/// <summary>
		/// 申请发布的时间
		/// </summary>
		public DateTime? Create { get; set; }
		public ApplyBaseInfo BaseInfo { get; set; }
		/// <summary>
		/// 申请的状态
		/// </summary>
		public AuditStatus Status { get; set; }
		/// <summary>
		/// 被召回的id
		/// </summary>
		public Guid? RecallId { get; set; }

		/// <summary>
		/// 假期主状态，根据人员情况联动
		/// </summary>
		public MainStatus MainStatus { get; set; }


	}
	public enum MainStatus
	{
		Normal = 0,
		/// <summary>
		/// 无效
		/// </summary>
		Invalid = 1,
		/// <summary>
		/// 休假计划
		/// </summary>
		IsPlan=2
	}

}
