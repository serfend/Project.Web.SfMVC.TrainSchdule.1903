using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;
using DAL.Entities.ApplyInfo;

namespace DAL.DTO.Apply
{
	public class ApplyDetailDTO:BaseEntity
	{
		public  ApplyBaseInfo BaseInfo { get; set; }
		public  ApplyRequest RequestInfo { get; set; }
		public DateTime Create { get; set; }
		public virtual IEnumerable<ApplyResponse> Response { get; set; }
		public AuditStatus Status { get; set; }
		public bool Hidden { get; set; }
	}
}
