using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entities
{
	
	public class Company
	{
		public string Name { get; set; }

		public bool IsPrivate { get; set; }
		/// <summary>
		/// 单位代码
		/// </summary>
		[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
		[Key]
		public string Code{get; set;}
	}
		
}
