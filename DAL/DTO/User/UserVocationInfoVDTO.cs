using DAL.Entities.Vacations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.DTO.User
{
	/// <summary>
	///
	/// </summary>
	public class UserVacationInfoVDto
	{
		/// <summary>
		/// 全年总天数
		/// </summary>
		public int YearlyLength { get; set; }

		/// <summary>
		/// 已休天数
		/// </summary>
		[NotMapped]
		public int ComsumeLength { get => YearlyLength - LeftLength; }

		/// <summary>
		/// 当前已休假次数
		/// </summary>
		public int NowTimes { get; set; }

		/// <summary>
		/// 剩休假天数
		/// </summary>
		public int LeftLength { get; set; }

		/// <summary>
		/// 已休路途次数
		/// </summary>
		public int OnTripTimes { get; set; }

		/// <summary>
		/// 全年最多可用路途次数
		/// </summary>
		public int MaxTripTimes { get; set; }

		/// <summary>
		/// 全年假期描述
		/// </summary>
		public string Description { get; set; }

		public IEnumerable<VacationAdditional> Additionals { get; set; }
	}
}