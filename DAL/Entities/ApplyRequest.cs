using System;
using System.Collections.Generic;
using System.Text;
using TrainSchdule.DAL.Entities.UserInfo;

namespace DAL.Entities
{
	/// <summary>
	/// 用户的申请请求
	/// </summary>
	public class ApplyRequest:BaseEntity
	{
		/// <summary>
		/// 休假天数
		/// </summary>
		public int xjts { get; set; }
		/// <summary>
		/// 路途天数
		/// </summary>
		public int ltts { get; set; }

	}
}
