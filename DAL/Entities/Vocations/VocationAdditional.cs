using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Entities.Vocations
{
	/// <summary>
	/// 额外假期
	/// </summary>
	public class VocationAdditional:BaseEntity
	{
		/// <summary>
		/// 假期名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 长度
		/// </summary>
		public int Length { get; set; }
		/// <summary>
		/// 开始时间
		/// </summary>
		public DateTime Start { get; set; }
		/// <summary>
		/// 说明
		/// </summary>
		public string Description { get; set; }
	}
}
