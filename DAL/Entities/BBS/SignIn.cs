using System;

namespace DAL.Entities.BBS
{
	public class SignIn : BaseEntity
	{
		/// <summary>
		/// 签到id
		/// </summary>
		public string SignId { get; set; }

		/// <summary>
		/// 签到时间
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// 连续签到天数
		/// </summary>
		public int ComboTimes { get; set; }
	}
}