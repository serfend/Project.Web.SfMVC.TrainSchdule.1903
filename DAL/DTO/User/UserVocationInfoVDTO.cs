using DAL.Entities.Vocations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.DTO.User
{

	/// <summary>
	/// 
	/// </summary>
	public class UserVocationInfoVDto
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
		public IEnumerable<VocationAdditional> Additionals { get; set; }
	}
}
