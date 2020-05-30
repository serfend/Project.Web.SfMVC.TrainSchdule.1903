using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.DTO.User.Social
{
	public class SettleDto
	{
		/// <summary>
		/// 本人所在地
		/// </summary>
		[AddressCodeOnProvince(false, ErrorMessage = "本人")]
		public virtual MomentDto Self { get; set; }

		/// <summary>
		/// 配偶所在地
		/// </summary>
		[AddressCodeOnProvince(false, ErrorMessage = "配偶")]
		public virtual MomentDto Lover { get; set; }

		/// <summary>
		/// 父母所在地
		/// </summary>
		[AddressCodeOnProvince(false, ErrorMessage = "父母")]
		public virtual MomentDto Parent { get; set; }

		/// <summary>
		/// 配偶的父母所在地
		/// </summary>
		[AddressCodeOnProvince(false, ErrorMessage = "配偶父母")]
		public virtual MomentDto LoversParent { get; set; }

		/// <summary>
		/// 年初因上一年度休事假消耗的天数
		/// </summary>
		public int PrevYearlyComsumeLength { get; set; }
	}
}