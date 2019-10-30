using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.UserInfo.Settle
{
	/// <summary>
	/// 居住情况，TODO 后续可将历史记录功能加入
	/// </summary>
	public class Moment: BaseEntity
	{
		/// <summary>
		/// 居住开始时间
		/// </summary>
		public DateTime Date { get; set; }
		/// <summary>
		/// 是否有效
		/// </summary>
		public bool Valid { get; set; }
		/// <summary>
		/// 居住地行政区划
		/// </summary>
		public AdminDivision Address { get; set; }
		/// <summary>
		/// 居住地详细地址
		/// </summary>
		public string AddressDetail { get; set; }
	}
}


