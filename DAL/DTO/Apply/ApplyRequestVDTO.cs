using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.Vacations;
using System;
using System.Collections.Generic;

namespace DAL.DTO.Apply
{
	public sealed class ApplyRequestVdto
	{
		public DateTime? StampLeave { get; set; }
		public DateTime? StampReturn { get; set; }

		/// <summary>
		/// 本次休假中跨越的假期的描述
		/// </summary>
		public string VacationDescriptions { get; set; }

		public int OnTripLength { get; set; }
		public int VacationLength { get; set; }
		public VacationType VacationType { get; set; }
		public AdminDivision VacationPlace { get; set; }
		public string VacationPlaceName { get; set; }
		public string Reason { get; set; }
		public Transportation ByTransportation { get; set; }
		public IEnumerable<VacationAdditional> VacationAdditionals { get; set; }
	}
}