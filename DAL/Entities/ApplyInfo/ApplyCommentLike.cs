using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.ApplyInfo
{
	public class ApplyCommentLike : BaseEntityInt
	{
		public DateTime Create { get; set; }
		public virtual User CreateBy { get; set; }
		public virtual ApplyComment Comment { get; set; }
	}
}