using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.UserInfo
{
	public class UserDiyInfo:BaseEntity
	{

		public string About { get; set; }
		/// <summary>
		/// 返回当前使用头像
		/// </summary>
		public virtual Avatar Avatar { get; set; }
	}
}
