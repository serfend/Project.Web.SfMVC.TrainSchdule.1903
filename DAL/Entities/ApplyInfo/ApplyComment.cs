using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entities.ApplyInfo
{
	/// <summary>
	/// 休假详情-评论
	/// </summary>
	public class ApplyComment : BaseEntityGuid
	{
		[ForeignKey("FromId")]
		public virtual User From { get; set; }
		public string FromId { get; set; }
		public DateTime Create { get; set; }
		public string Content { get; set; }
		public DateTime LastModify { get; set; }
		[ForeignKey("ModifyById")]
		public virtual User ModifyBy { get; set; }
		public string ModifyById { get; set; }
		public int Likes { get; set; }

		/// <summary>
		/// 作用到
		/// </summary>
		public string Apply { get; set; }
	}
}