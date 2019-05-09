using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using DAL.DTO.User;

namespace DAL.DTO.Apply
{
	public sealed class ApplyDetailDto:BaseEntity
	{
		public UserSummaryDto Base { get; set; }
		public Entities.Company Company { get; set; }
		public Duties Duties { get; set; }
		public UserSocialInfo Social { get; set; }
		public  ApplyRequest RequestInfo { get; set; }
		public DateTime?Create { get; set; }
		public IEnumerable<ApplyResponseDto> Response { get; set; }
		public AuditStatus Status { get; set; }
		public bool Hidden { get; set; }
	}
}
