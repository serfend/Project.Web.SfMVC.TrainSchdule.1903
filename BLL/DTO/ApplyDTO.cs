using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;
using Newtonsoft.Json;

namespace BLL.DTO
{
	public class ApplyDTO
	{
		[JsonProperty("id")]
		public Guid Id { get; set; }
		/// <summary>
		/// 申请人姓名
		/// </summary>
		[JsonProperty("from")]
		public string From { get; set; }
		/// <summary>
		/// 申请人用户名
		/// </summary>
		[JsonProperty("fromUserName")]
		public string FromUserName { get; set; }
		/// <summary>
		/// 申请所在单位的路径
		/// </summary>
		[JsonProperty("company")]
		public string Company { get; set; }

		/// <summary>
		/// 申请创建的时间
		/// </summary>
		[JsonProperty("create")]
		public DateTime Create { get; set; }
		/// <summary>
		/// 申请通过的状态
		/// </summary>
		[JsonProperty("status")]
		public AuditStatus Status { get; set; }
		/// <summary>
		/// 当前申请所在层级的单位的路径
		/// </summary>
		[JsonProperty("current")]
		public string Current { get; set; }
		/// <summary>
		/// 申请的详细审批情况
		/// </summary>
		[JsonProperty("progress")]
		public IEnumerable<ApplyResponseDTO> Progress { get; set; }
		[JsonProperty("detail")]
		public ApplyDetailDTO Detail { get; set; }
	}
}
