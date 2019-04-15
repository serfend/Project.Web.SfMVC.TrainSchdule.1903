using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;

namespace BLL.DTO
{
	public class ApplyResponseDTO
	{
		/// <summary>
		/// 处理单位的名称
		/// </summary>
		public string Company { get; set; }
		/// <summary>
		/// 处理人姓名
		/// </summary>
		public string AuditBy { get; set; }
		/// <summary>
		/// 处理人用户名
		/// </summary>
		public string AuditUserName { get; set; }
		/// <summary>
		/// 处理时间
		/// </summary>
		public DateTime HdlStamp { get; set; }
		/// <summary>
		/// 处理状态
		/// </summary>
		public Auditing Status { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; }
	}
}
