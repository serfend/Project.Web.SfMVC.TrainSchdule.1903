using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;

namespace BLL.DTO
{
	public class ApplyDTO
	{
		public Guid Id { get; set; }
		/// <summary>
		/// 申请人姓名
		/// </summary>
		public string From { get; set; }
		/// <summary>
		/// 申请人用户名
		/// </summary>
		public string FromUserName { get; set; }
		/// <summary>
		/// 申请所在单位的路径
		/// </summary>
		public string Company { get; set; }

		/// <summary>
		/// 申请创建的时间
		/// </summary>
		public DateTime Create { get; set; }
		/// <summary>
		/// 申请通过的状态
		/// </summary>
		public AuditStatus Status { get; set; }
		/// <summary>
		/// 当前申请所在层级的单位的路径
		/// </summary>
		public string Current { get; set; }
		/// <summary>
		/// 申请的详细审批情况
		/// </summary>
		public IEnumerable<ApplyResponseDTO> Progress { get; set; }
		public ApplyDetailDTO Detail { get; set; }
	}
}
