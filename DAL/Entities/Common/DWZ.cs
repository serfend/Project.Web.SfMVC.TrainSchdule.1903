using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entities.Common
{
	public class ShortUrl : BaseEntity, ICreateClientInfo
	{
		/// <summary>
		/// 原始网址
		/// </summary>
		public string Target { get; set; }

		public string Key { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime Create { get; set; }

		/// <summary>
		/// 有效期限
		/// </summary>
		public DateTime Expire { get; set; }

		/// <summary>
		/// 创建人
		/// </summary>
		public virtual UserInfo.User CreateBy { get; set; }

		public string Ip { get; set; }
		public string Device { get; set; }
		public string UA { get; set; }
	}

	public class ShortUrlStatistics : BaseEntity, ICreateClientInfo
	{
		public virtual ShortUrl Url { get; set; }

		/// <summary>
		/// 访客
		/// </summary>
		public virtual UserInfo.User ViewBy { get; set; }

		public DateTime Create { get; set; }
		public string Ip { get; set; }
		public string Device { get; set; }
		public string UA { get; set; }
	}
}