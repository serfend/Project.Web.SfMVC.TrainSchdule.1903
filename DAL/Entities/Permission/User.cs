using System;
using System.Collections.Generic;
using System.Text;

namespace TrainSchdule.DAL.Entities.UserInfo.Permission
{
	public class User
	{
		public PermissionRange 基本信息 { get; set; }
		public PermissionRange 社会关系 { get; set; }
		public PermissionRange 职务信息 { get; set; }
	}
}
