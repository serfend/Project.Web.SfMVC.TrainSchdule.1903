using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.UserInfo.Permission
{
	public class User : BaseEntity
	{
		public virtual PermissionRange 基本信息 { get; set; }
		public virtual PermissionRange 社会关系 { get; set; }
		public virtual PermissionRange 职务信息 { get; set; }
	}
}
