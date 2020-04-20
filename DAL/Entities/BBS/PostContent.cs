﻿using DAL.Entities.UserInfo;
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

		/// <summary>
		/// 阅读次数
		/// </summary>
		public int ViewCount { get; set; }

		/// <summary>
		/// 动态图片文件id，通过FileEngine获取对应文件，以##分割
		/// </summary>
		public string Images { get; set; }

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