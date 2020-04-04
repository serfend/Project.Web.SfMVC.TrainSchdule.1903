using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
	public class VocationDescription
	{
		[Key]
		public int Id { get; set; }

		public DateTime Start { get; set; }

		public int Length { get; set; }
		public string Name { get; set; }
	}
}