using DAL.Entities.UserInfo.DiyInfo;
using System.Collections;
using System.Collections.Generic;

namespace DAL.Entities.UserInfo
{
	public class UserDiyInfo : BaseEntityGuid
	{
		public string About { get; set; }

		/// <summary>
		/// 返回当前使用头像6
		/// </summary>
		public virtual Avatar Avatar { get; set; }

		/// <summary>
		/// 第三方账号列表
		/// </summary>
		public virtual IEnumerable<ThirdpardAccount> ThirdpardAccount { get; set; }
	}
}