﻿using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;
using DAL.Entities.ApplyInfo;

namespace DAL.DTO.Apply
{
	public class ApplySummaryDTO : BaseEntity
	{
		public ApplyBaseInfoDTO Base { get; set; }
		public string NowAuditCompany { get; set; }
		public string VocationPlace { get; set; }
		public string HomePlace { get; set; }
		public DateTime StampLeave { get; set; }
		public DateTime StampReturn { get; set; }
		public DateTime Create { get; set; }
		
		public AuditStatus Status { get; set; }

	}
}