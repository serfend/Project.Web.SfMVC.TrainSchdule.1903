using DAL.Entities.BBS;
using System;

namespace DAL.Entities.Game_r3
{
	public class User : BaseEntity
	{
		/// <summary>
		/// 用户游戏Id
		/// </summary>
		public string GameId { get; set; }

		/// <summary>
		/// 是否启用
		/// </summary>
		public bool Enable { get; set; }

		/// <summary>
		/// 领取间隔（间隔时间内对所有的可领取代码进行领取操作）单位ms
		/// </summary>
		public long HandleInterval { get; set; }

		/// <summary>
		/// 上次处理领取时间
		/// </summary>
		public long LastHandleStamp { get; set; }

		/// <summary>
		/// 用户上次登录时间
		/// </summary>
		public DateTime LastLogin { get; set; }

		/// <summary>
		/// 上次登录/签到时间
		/// </summary>

		public virtual SignIn LastSignIn { get; set; }
	}

	public class UserInfo : BaseEntity
	{
		/// <summary>
		/// 用户
		/// </summary>
		public virtual User User { get; set; }

		/// <summary>
		/// 信息生效时间
		/// </summary>
		public DateTime DateTime { get; set; }

		/// <summary>
		/// 昵称
		/// </summary>
		public string NickName { get; set; }

		/// <summary>
		/// 等级
		/// </summary>
		public string Level { get; set; }
	}
}