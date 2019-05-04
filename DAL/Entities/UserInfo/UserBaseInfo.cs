using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities.UserInfo;

namespace DAL.Entities.UserInfo
{
	public class UserBaseInfo : BaseEntity
	{

		/// <summary>
		/// 授权码
		/// </summary>
		public string AuthKey { get; set; }
		public string RealName { get; set; }
		public string Avatar { get; set; }
		public GenderEnum Gender { get; set; }
		public bool PrivateAccount { get; set; }
	}
	public enum GenderEnum
	{
		Unknown = 0,
		Male = 1,
		Female = 2,

	}
}
