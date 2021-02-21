using System;

namespace DAL.Entities.UserInfo
{
	public class UserApplicationInfo : BaseEntityGuid
	{
		/// <summary>
		/// 用户注册时的授权人id
		/// </summary>
		public string InvitedBy { get; set; }

		public virtual Permissions Permission { get; set; }
		public DateTime? Create { get; set; }
		public string Email { get; set; }
		public string AuthKey { get; set; }
		/// <summary>
		/// 用户被移除的原因
		/// </summary>
		public string UserRemoveReason { get; set; }

		public virtual UserApplicationSetting ApplicationSetting { get; set; }
	}
}