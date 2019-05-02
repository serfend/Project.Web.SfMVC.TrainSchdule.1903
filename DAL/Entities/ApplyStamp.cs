using System;
using System.Collections.Generic;
using System.Text;
using TrainSchdule.DAL.Entities.UserInfo;

namespace DAL.Entities
{
	public class ApplyStamp:BaseEntity
	{
		/// <summary>
		/// 离队时间
		/// </summary>
		public DateTime ldsj { get; set; }
		/// <summary>
		/// 归队时间
		/// </summary>
		public DateTime gdsj { get; set; }
	}
}
