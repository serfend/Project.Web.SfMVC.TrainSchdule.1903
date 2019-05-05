using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using DAL.Entities.UserInfo;

namespace DAL.Entities
{
	public class Duties 
	{
		[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
		[Key]
		public string Code { get; set; }
		/// <summary>
		/// 职务的名称
		/// </summary>
		public string Name { get; set; }
	}
}
