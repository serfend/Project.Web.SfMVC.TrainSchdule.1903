using DAL.Entities.UserInfo;

namespace DAL.DTO.User
{
	/// <summary>
	/// 仅提供基本信息
	/// </summary>
	public class UserSummaryDto
	{
		/// <summary>
		/// 用户账号
		/// </summary>
		public string Id { get; set; }
		/// <summary>
		/// 真实姓名
		/// </summary>
		public string RealName { get; set; }
		/// <summary>
		/// 简介
		/// </summary>
		public string About { get; set; }

		/// <summary>
		/// 性别
		/// </summary>
		public GenderEnum Gender { get; set; }
		/// <summary>
		/// 头像文件地址
		/// </summary>
		public string Avatar { get; set; }
		/// <summary>
		/// 单位代码
		/// </summary>
		public string CompanyCode { get; set; }
		/// <summary>
		/// 职务代码
		/// </summary>
		public int? DutiesCode { get; set; }
		/// <summary>
		/// 单位名称
		/// </summary>
		public string CompanyName { get; set; }
		/// <summary>
		/// 职务名
		/// </summary>
		public string DutiesName { get; set; }
		public UserAction LastLogin { get; set; }
		/// <summary>
		/// 邀请人
		/// </summary>
		public string InviteBy { get; set; }
		/// <summary>
		/// 当前密码是否是初始密码
		/// </summary>
		public bool IsInitPassword { get; set; }
	}
}
