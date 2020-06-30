using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.UserInfo.Settle
{
	/// <summary>
	/// 居住情况
	/// </summary>
	public class Settle : BaseEntityGuid
	{
		/// <summary>
		/// 本人所在地
		/// </summary>
		[AddressCodeOnProvince(false, ErrorMessage = "本人")]
		public virtual Moment Self { get; set; }

		/// <summary>
		/// 配偶所在地
		/// </summary>
		[AddressCodeOnProvince(false, ErrorMessage = "配偶")]
		public virtual Moment Lover { get; set; }

		/// <summary>
		/// 父母所在地
		/// </summary>
		[AddressCodeOnProvince(false, ErrorMessage = "父母")]
		public virtual Moment Parent { get; set; }

		/// <summary>
		/// 配偶的父母所在地
		/// </summary>
		[AddressCodeOnProvince(false, ErrorMessage = "配偶父母")]
		public virtual Moment LoversParent { get; set; }

		/// <summary>
		/// 全年发生变化的记录
		/// </summary>
		public virtual IEnumerable<AppUsersSettleModefyRecord> PrevYealyLengthHistory { get; set; }

		/// <summary>
		/// 年初因上一年度休事假消耗的天数
		/// </summary>
		public int PrevYearlyComsumeLength { get; set; }
	}
}