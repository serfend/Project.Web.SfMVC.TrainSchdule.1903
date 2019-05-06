using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;

namespace DAL.DTO.Apply
{
	public class ApplyRequestDTO
	{
		public DateTime StampLeave { get; set; }
		public DateTime StampReturn { get; set; }
		public int OnTripLength { get; set; }
		public int VocationLength { get; set; }
		public string VocationType { get; set; }
		public virtual AdminDivision VocationPlace { get; set; }
		public string Reason { get; set; }
	}
}
