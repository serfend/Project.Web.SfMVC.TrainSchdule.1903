using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.BBS
{
	/// <summary>
	/// 动态（朋友圈模型）
	/// </summary>
	public class Post : PostContent
	{
		/// <summary>
		/// 动态的回复
		/// </summary>
		public virtual IEnumerable<PostContent> ChildContents { get; set; }
	}
}
