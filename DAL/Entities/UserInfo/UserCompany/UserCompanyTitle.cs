using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Entities.UserInfo
{
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
		/// 排序级别
		/// </summary>
		public int Level { get; set; }

		/// <summary>
		/// 职务等级对应的休假天数（专用）
		/// </summary>
		public int VacationDay { get; set; }

		/// <summary>
		/// 是否决定性影响休假天数
		/// </summary>
		public bool EnableVacationDay { get; set; }

		/// <summary>
		/// 是否禁用假期
		/// </summary>
		public bool DisableVacation { get; set; }

		/// <summary>
		/// 职务等级类别，用于统计中进行分类（专用）
		/// </summary>
		public string TitleType { get; set; }
	}
}