using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Entities
{
	public class AdminDivision
	{

		[Key]
		public int Code { get; set; }
		public string Name { get; set; }
	}
}
