using System;

namespace DAL.Entities.Game_r3
{
	public class GiftCode : BaseEntity
	{
		/// <summary>
		/// 礼包码
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// 有效性
		/// </summary>
		public bool Valid { get; set; }

		/// <summary>
		/// 礼品码失效代码及原因
		/// </summary>
		public string StatusDescription { get; set; }

		/// <summary>
		/// 分享时间
		/// </summary>
		public DateTime ShareTime { get; set; }

		/// <summary>
		/// 自动识别失效时间
		/// </summary>
		public DateTime InvalidTime { get; set; }

		/// <summary>
		/// 分享人
		/// </summary>
		public string ShareBy { get; set; }
	}

	public class GainGiftCode : BaseEntity
	{
		public virtual User User { get; set; }
		public long GainStamp { get; set; }
		public virtual GiftCode Code { get; set; }
	}
}