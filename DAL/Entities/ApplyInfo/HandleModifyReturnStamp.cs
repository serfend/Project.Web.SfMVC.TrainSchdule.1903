using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.ApplyInfo
{
	public class HandleModifyReturnStamp : BaseEntityGuid
	{
		/// <summary>
		/// 召回原因
		/// </summary>
		public string Reason { get; set; }

		public virtual User HandleBy { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime Create { get; set; }

		/// <summary>
		/// 归队时间
		/// </summary>
		public DateTime ReturnStamp { get; set; }
	}
}