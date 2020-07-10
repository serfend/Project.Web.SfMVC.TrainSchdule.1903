using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Entities.Common.DataDictionary
{
	public class CommonDataGroup : BaseEntityInt
	{
		/// <summary>
		/// 名称
		/// </summary>
		[Key]
		public string Name { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime Create { get; set; }
	}
}