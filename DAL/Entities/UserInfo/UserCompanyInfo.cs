﻿using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities.UserInfo;

namespace DAL.Entities.UserInfo
{
	public class UserCompanyInfo : BaseEntity
	{
		/// <summary>
		/// 用户所处的单位
		/// </summary>
		public virtual Company Company { get; set; }
		public virtual Duties Duties { get; set; }
	}
}