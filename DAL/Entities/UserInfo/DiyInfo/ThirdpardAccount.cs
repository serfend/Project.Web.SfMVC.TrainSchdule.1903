using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.UserInfo.DiyInfo
{
	/// <summary>
	/// 第三方账号
	/// </summary>
	public class ThirdpardAccount : BaseEntityGuid
	{
		public string Account { get; set; }
		public string Token { get; set; }

		/// <summary>
		/// 第三方平台名称
		/// </summary>
		public string ThirdpardPlatformName { get; set; }

		public string NickName { get; set; }
	}
}