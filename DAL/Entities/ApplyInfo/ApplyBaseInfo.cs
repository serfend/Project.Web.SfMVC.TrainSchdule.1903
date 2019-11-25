﻿using System;
using DAL.Entities.UserInfo;

namespace DAL.Entities.ApplyInfo
{
	public class ApplyBaseInfo : BaseEntity
	{
		public string RealName { get; set; }
		public string CompanyName { get; set; }
		public string DutiesName { get; set; }
		/// <summary>
		/// 申请创建的目标人
		/// </summary>
		public virtual User From { get; set; }
		public virtual Company Company { get; set; }
		public virtual Duties Duties { get; set; }
		public virtual UserSocialInfo Social { get; set; }
		public DateTime CreateTime { get; set; }
		/// <summary>
		/// 申请由何人创建
		/// </summary>
		public virtual User CreateBy{get;set;}
	}
}
