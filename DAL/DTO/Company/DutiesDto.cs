using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.DTO.Company
{
	public class DutiesDto
	{
		public int Code { get; set; }

		/// <summary>
		/// 职务的名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 是否是主官
		/// </summary>
		public bool IsMajorManager { get; set; }

		/// <summary>
		/// 职务标签
		/// </summary>
		public IEnumerable<string> Tags { get; set; }
	}
}