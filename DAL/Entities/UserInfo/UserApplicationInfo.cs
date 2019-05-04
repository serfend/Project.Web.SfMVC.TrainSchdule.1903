using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities.UserInfo.Permission;

namespace DAL.Entities.UserInfo
{
	public class UserApplicationInfo:BaseEntity
	{
		/// <summary>
		/// 用户注册时的授权人id
		/// </summary>
		public string InvitedBy { get; set; }

		public virtual Permissions Permission { get; set; }
		public string About { get; set; }
		public DateTime Create { get; set; }
		public string Email { get; set; }
		public string AuthKey { get; set; }

	}
}
