using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;

namespace DAL.DTO.Apply
{
	public class ApplyDetailDTO:BaseEntity
	{
		public virtual Entities.Company Company { get; set; }
		public virtual Duties Duties { get; set; }
		public virtual UserSocialInfo Social { get; set; }
		public  ApplyRequest RequestInfo { get; set; }
		public DateTime?Create { get; set; }
		public virtual IEnumerable<ApplyResponseDTO> Response { get; set; }
		public AuditStatus Status { get; set; }
		public bool Hidden { get; set; }
	}
}
