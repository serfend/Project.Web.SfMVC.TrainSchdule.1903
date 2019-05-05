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

		/// <summary>
		/// 由<see cref="Code"/>决定的父单位
		/// </summary>
		public virtual Company Parent { get; set; }
		public bool IsPrivate { get; set; }
		/// <summary>
		/// 单位代码
		/// </summary>
		[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
		[Key]
		public string Code{get; set;}
	}
		
}
