using System;
using System.Collections.Generic;
using System.Text;

namespace TrainSchdule.DAL.Entities.UserInfo.Permission
{
	public class Apply:BaseEntity
	{
		public virtual PermissionRange 申请信息 { get; set; }
		public virtual PermissionRange 审批流 { get; set; }

	}
}
