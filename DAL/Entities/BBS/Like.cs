using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.BBS
{
	/// <summary>
	/// 点赞
	/// </summary>
	public class Like : BaseEntityGuid
	{
		public virtual PostContent Content { get; set; }
		public DateTime Create { get; set; }
		public virtual User CreateBy { get; set; }
	}
}