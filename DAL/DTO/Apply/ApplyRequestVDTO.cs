using DAL.Entities;
using System;

namespace DAL.DTO.Apply
{
	public sealed class ApplyRequestVdto
	{
		public DateTime?StampLeave { get; set; }
		public DateTime?StampReturn { get; set; }
		public int OnTripLength { get; set; }
		public int VocationLength { get; set; }
		public string VocationType { get; set; }
		public AdminDivision VocationPlace { get; set; }
		public string Reason { get; set; }
	}
}
