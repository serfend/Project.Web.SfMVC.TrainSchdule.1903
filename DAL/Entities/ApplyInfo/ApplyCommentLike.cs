using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entities.ApplyInfo
{
	public class ApplyCommentLike : BaseEntityInt
	{
		public DateTime Create { get; set; }
		[ForeignKey("CreateById")]
		public virtual User CreateBy { get; set; }
		public string CreateById { get; set; }
		[ForeignKey("CommentId")]
		public virtual ApplyComment Comment { get; set; }
		public Guid? CommentId { get; set; }
	}
}