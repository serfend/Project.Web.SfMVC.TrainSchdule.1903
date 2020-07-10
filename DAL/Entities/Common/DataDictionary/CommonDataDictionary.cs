using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entities.Common.DataDictionary
{
	public class CommonDataDictionary : BaseEntityInt
	{
		/// <summary>
		/// 字典组
		/// </summary>
		public string GroupName { get; set; }

		/// <summary>
		/// 属性名称
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// 属性值
		/// </summary>
		public int Value { get; set; }

		/// <summary>
		/// 别名
		/// </summary>
		public string Alias { get; set; }

		public string Description { get; set; }
		public string Color { get; set; }
	}
}