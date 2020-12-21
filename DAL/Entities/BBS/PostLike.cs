using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.BBS
{
	/// <summary>
	/// 点赞
	/// </summary>
	public class PostInteractStatus : BaseEntityGuid
	{
		public virtual PostContent Content { get; set; }
		public DateTime Create { get; set; }
		/// <summary>
		/// 交互状态
		/// </summary>
		public PostStatus PostStatus { get; set; }
		public virtual User CreateBy { get; set; }
	}
	public enum PostStatus
    {
		None=0,
		Read=1,
		Like=2,
    }
}