using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities.UserInfo
{
	public class UserBaseInfo : BaseEntity
	{
		[Required(ErrorMessage = "未输入身份证")]

		/// <summary>
		/// 居民身份证
		/// </summary>
		public string Cid { get; set; }

		[Required(ErrorMessage = "未输入真实姓名")]
		public string RealName { get; set; }

		[Required(ErrorMessage = "未输入性别")]
		public GenderEnum Gender { get; set; }

		[Required(ErrorMessage = "未输入籍贯")]
		public string Hometown { get; set; }

		[Required(ErrorMessage = "未输入工作时间")]
		/// <summary>
		/// 工作/入伍时间
		/// </summary>
		public DateTime Time_Work { get; set; }

		[Required(ErrorMessage = "未输入生日")]

		/// <summary>
		/// 出生日期
		/// </summary>
		public DateTime Time_BirthDay { get; set; }

		[Required(ErrorMessage = "未输入党团时间")]

		/// <summary>
		/// 党团时间
		/// </summary>
		public DateTime Time_Party { get; set; }

		public bool PrivateAccount { get; set; }

		/// <summary>
		/// 是否修改过密码
		/// </summary>
		public bool PasswordModefy { get; set; }
	}

	public enum GenderEnum
	{
		[Description("未知")]
		Unknown = 0,

		[Description("男")]
		Male = 1,

		[Description("女")]
		Female = 2,
	}
}