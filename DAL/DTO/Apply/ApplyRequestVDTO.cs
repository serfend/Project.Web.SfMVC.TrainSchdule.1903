using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.Vocations;
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
		public string VocationDescriptions { get; set; }

		public int OnTripLength { get; set; }
		public int VocationLength { get; set; }
		public string VocationType { get; set; }
		public AdminDivision VocationPlace { get; set; }
		public string VocationPlaceName { get; set; }
		public string Reason { get; set; }
		public Transportation ByTransportation { get; set; }
		public IEnumerable<VocationAdditional> VocationAdditionals { get; set; }
	}
}