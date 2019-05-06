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
		public string Name { get; set; }
	}
}
