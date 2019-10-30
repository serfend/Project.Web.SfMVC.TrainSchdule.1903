﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.UserInfo
{
	public class UserApplicationSetting:BaseEntity
	{
		/// <summary>
		/// 用户两次提交申请的时间应间隔不少于一定时间
		/// </summary>
		public DateTime? LastSubmitApplyTime { get; set; }

	}
}