using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities.UserInfo
{
	public class UserBaseInfo : BaseEntity
	{
		[Required]
		/// <summary>
		/// 居民身份证
		/// </summary>
		public string Cid { get; set; }
		public string RealName { get; set; }
		
		public GenderEnum Gender { get; set; }
		/// <summary>
		/// 工作/入伍时间
		/// </summary>
		public DateTime Time_Work { get; set; }
		/// <summary>
		/// 出生日期
		/// </summary>
		public DateTime Time_BirthDay { get; set; }
		/// <summary>
		/// 党团时间
		/// </summary>
		public DateTime Time_Party { get; set; }
		public bool PrivateAccount { get; set; }

	}
	public enum GenderEnum
	{
		Unknown = 0,
		Male = 1,
		Female = 2,

	}
}
