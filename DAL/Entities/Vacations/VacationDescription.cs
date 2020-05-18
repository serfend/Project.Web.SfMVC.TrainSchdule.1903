using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
	[Table("VocationDescriptions")]
	public class VacationDescription
	{
		[Key]
		public int Id { get; set; }

		public DateTime Start { get; set; }

		public int Length { get; set; }
		public string Name { get; set; }
	}
}