using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.DTO.User.Social
{
	public class SocialDto
	{
		/// <summary>
		/// 联系方式
		/// </summary>
		[RegularExpression(@"^1[3456789]\d{9}$", ErrorMessage = "手机号格式错误")]
		public string Phone { get; set; }

		/// <summary>
		///
		/// </summary>
		public SettleDto Settle { get; set; }

		/// <summary>
		/// 家庭地址代码
		/// </summary>
		public AdminDivision Address { get; set; }

		/// <summary>
		/// 家庭详细地址
		/// </summary>
		public string AddressDetail { get; set; }
	}
}