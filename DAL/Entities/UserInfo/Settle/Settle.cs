using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.UserInfo.Settle
{
	/// <summary>
	/// 居住情况
	/// </summary>
	public class Settle : BaseEntity
	{
		/// <summary>
		/// 本人所在地
		/// </summary>
		public virtual Moment Self { get; set; }
		/// <summary>
		/// 配偶所在地
		/// </summary>
		public virtual Moment Lover { get; set; }
		/// <summary>
		/// 父母所在地
		/// </summary>
		public virtual Moment Parent { get; set; }


	}
}


