using DAL.Entities.UserInfo;
using System;

namespace DAL.Entities.BBS
{
	public class PostContent : BaseEntity
	{
		/// <summary>
		/// 回复何动态
		/// </summary>
		public virtual PostContent ReplySubject { get; set; }

		public string Title { get; set; }
		public string Contents { get; set; }
		public DateTime Create { get; set; }

		/// <summary>
		/// 回复的创建人
		/// </summary>
		public virtual User CreateBy { get; set; }

		/// <summary>
		/// 回复的作用人，可为null
		/// </summary>
		public virtual User ReplyTo { get; set; }
	}
}