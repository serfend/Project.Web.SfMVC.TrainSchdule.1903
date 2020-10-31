using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities.UserInfo
{
	public class UserCompanyInfo : BaseEntityGuid
	{
		/// <summary>
		/// 用户所处的单位
		/// </summary>
		public virtual Company Company { get; set; }

		//[Required(ErrorMessage = "未输入职务信息")]
		public virtual Duties Duties { get; set; }

		/// <summary>
		/// 职务等级
		/// </summary>
		//[Required(ErrorMessage = "未输入职务等级")]
		public virtual UserCompanyTitle Title { get; set; }

		/// <summary>
		/// 职务等级时间
		/// </summary>
		//[Required(ErrorMessage = "未输入职务等级时间")]
		public DateTime? TitleDate { get; set; }
	}
}