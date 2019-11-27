using DAL.DTO.Apply;
using DAL.DTO.User;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

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
