using DAL.Entities;
using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Apply
{
	/// <summary>
	/// 最基本信息
	/// </summary>
    public class ApplyShadowDto: BaseEntityGuid
	{
		public string AuditLeader { get; set; }

		/// <summary>
		/// 申请发布的时间
		/// </summary>
		public DateTime? Create { get; set; }

		/// <summary>
		/// 申请的状态
		/// </summary>
		public AuditStatus Status { get; set; }

		/// <summary>
		/// 假期主状态，根据人员情况联动
		/// </summary>
		public MainStatus MainStatus { get; set; }

		/// <summary>
		/// 休假落实状态
		/// </summary>
		public ExecuteStatus ExecuteStatus { get; set; }

		/// <summary>
		/// 休假落实状态，需要联动修改<see cref="ExecuteStatus"/>
		/// </summary>

		public Guid? ExecuteStatusDetailId { get; set; }

		/// <summary>
		/// 被召回的id
		/// </summary>
		public Guid? RecallId { get; set; }
	}
}
