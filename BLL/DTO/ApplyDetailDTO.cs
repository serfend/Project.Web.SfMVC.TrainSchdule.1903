using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;
using Newtonsoft.Json;

namespace BLL.DTO
{
	public class ApplyDetailDTO
	{
		/// <summary>
		/// 申请人ID
		/// </summary>
		[JsonProperty("fromId")]
		public Guid FromId { get; set; }
		/// <summary>
		/// 申请的具体内容
		/// </summary>
		[JsonProperty("request")]
		public ApplyRequest Request { get; set; }
		/// <summary>
		/// 休假类别
		/// </summary>
		[JsonProperty("xjlb")]
		public string Xjlb { get; set; }
		[JsonProperty("stamp")]
		public ApplyStamp Stamp { get; set; }
	}
}
