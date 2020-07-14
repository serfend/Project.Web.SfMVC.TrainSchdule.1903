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

		[Required(ErrorMessage = "未输入职务信息")]
		public virtual Duties Duties { get; set; }

		/// <summary>
		/// 职务等级
		/// </summary>
		[Required(ErrorMessage = "未输入职务等级")]
		public virtual UserCompanyTitle Title { get; set; }

		/// <summary>
		/// 职务等级时间
		/// </summary>
		[Required(ErrorMessage = "未输入职务等级时间")]
		public DateTime? TitleDate { get; set; }
	}

	/// <summary>
	/// 职务等级
	/// </summary>
	public class UserCompanyTitle
	{
		[Key]
		public int Code { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 级别
		/// </summary>
		public int Level { get; set; }

		/// <summary>
		/// 职务等级对应的休假天数（专用）
		/// </summary>
		public int VacationDay { get; set; }

		/// <summary>
		/// 职务等级类别，用于统计中进行分类（专用）
		/// </summary>
		public string TitleType { get; set; }
	}
}