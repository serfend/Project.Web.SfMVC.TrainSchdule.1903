using DAL.Entities;
using System;
using System.Collections.Generic;

namespace DAL.DTO.System
{
	public class VocationDateDto
	{
		public IEnumerable<VocationDescription> Descriptions { get; set; }
		public int VocationDays { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}