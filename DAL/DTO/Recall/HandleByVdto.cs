using DAL.DTO.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.DTO.Recall
{
	public class HandleByVdto
	{
		public string Reason { get; set; }
		public UserSummaryDto HandleBy { get; set; }
		public DateTime Create { get; set; }
		public DateTime ReturnStamp { get; set; }
		public Guid Apply { get; set; }
	}
}