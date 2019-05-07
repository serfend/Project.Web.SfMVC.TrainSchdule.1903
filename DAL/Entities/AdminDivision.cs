using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL.Entities
{
	public class AdminDivision
	{

		[Key]
		[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
		public int Code { get; set; }
		public int ParentCode { get; set; }
		/// <summary>
		/// 北京北京市海淀区
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 海淀区
		/// </summary>
		public string ShortName { get; set; }
	}
}
