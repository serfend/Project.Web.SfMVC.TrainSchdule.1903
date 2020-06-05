using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.Common
{
	/// <summary>
	/// 版本记录
	/// </summary>
	public class ApplicationUpdateRecord : BaseEntity
	{
		/// <summary>
		/// 版本号
		/// </summary>
		public string Version { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime Create { get; set; }
	}
}