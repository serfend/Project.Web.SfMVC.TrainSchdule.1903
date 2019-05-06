﻿using DAL.Entities.UserInfo;

namespace DAL.Entities.ApplyInfo
{
	public class ApplyBaseInfo
	{
		public virtual User From { get; set; }
		public virtual Company Company { get; set; }
		public virtual Duties Duties { get; set; }
		public virtual UserSocialInfo Social { get; set; }
	}
}
