using System;

namespace DAL.DTO.Apply
{
	public class ApplyVdto
	{
		public Guid BaseInfoId { get; set; }
		public Guid RequestInfoId { get; set; }
		public bool IsPlan { get; set; }
		public string EntityType { get; set; }
	}
}