﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TrainSchdule.DAL.Entities.UserInfo.Permission
{
	public class Company : BaseEntity
	{
		public virtual PermissionRange 单位信息 { get; set; }
	}
}
