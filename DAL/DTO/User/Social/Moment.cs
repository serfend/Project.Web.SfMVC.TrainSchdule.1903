using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.DTO.User.Social
{
	public class MomentDto
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
		public virtual AdminDivision Address { get; set; }

		/// <summary>
		/// 居住地详细地址
		/// </summary>
		public string AddressDetail { get; set; }
	}
}