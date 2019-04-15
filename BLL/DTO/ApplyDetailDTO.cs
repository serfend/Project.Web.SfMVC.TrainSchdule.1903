using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;

namespace BLL.DTO
{
	public class ApplyDetailDTO
	{
		/// <summary>
		/// 申请人ID
		/// </summary>
		public Guid FromId { get; set; }
		/// <summary>
		/// 申请的具体内容
		/// </summary>
		public ApplyRequest Request { get; set; }
		/// <summary>
		/// 休假类别
		/// </summary>
		public string xjlb { get; set; }
		public ApplyStamp Stamp { get; set; }
	}
}
