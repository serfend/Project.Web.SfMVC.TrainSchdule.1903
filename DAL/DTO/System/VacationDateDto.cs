using DAL.Entities;
using System;
using System.Collections.Generic;

namespace DAL.DTO.System
{
	public class VacationDateDto
	{
		public IEnumerable<VacationDescription> Descriptions { get; set; }
		public int VacationDays { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}