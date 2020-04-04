using DAL.DTO.User;
using System;

namespace DAL.DTO.Recall
{
	public class RecallOrderVDto
	{
		public string Reason { get; set; }
		public UserSummaryDto RecallBy { get; set; }
		public DateTime Create { get; set; }
		public DateTime ReturnStamp { get; set; }
		public Guid Apply { get; set; }
	}
}