using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.BBS
{
	/// <summary>
	/// 站内信内容
	/// </summary>
	public class MessageContent : BaseEntityGuid
	{
		public string Title { get; set; }

		/// <summary>
		/// 当存在url时应自动标注
		/// </summary>
		public string Content { get; set; }

		/// <summary>
		/// 图文配图的链接
		/// </summary>
		public string Logo { get; set; }

		public User CreateBy { get; set; }

		/// <summary>
		/// 如果是特殊发送人，则显示特殊发送人名称。<see cref="CreateBy"/>此时须为null
		/// </summary>
		public string CreateByAlias { get; set; }

		public DateTime Create { get; set; }

		/// <summary>
		/// 有效期限，用户在信息创建后多久内登录可接收到此消息
		/// </summary>
		public TimeSpan ValidStartLength { get; set; }

		/// <summary>
		/// 有效期限，用户在信息创建前多久内登录可接收到此消息
		/// </summary>
		public TimeSpan ValidEndLength { get; set; }
	}

	public enum MessageType
	{
		Normal = 0,
		System = 1,
		Notice = 2,
		Admin = 4
	}
}