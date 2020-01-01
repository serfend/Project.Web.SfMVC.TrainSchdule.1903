using System;
using System.Collections.Generic;
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
		/// <summary>
		/// 配偶的父母所在地
		/// </summary>
		public virtual Moment LoversParent { get; set; }
		/// <summary>
		/// 本年原始休假天数
		/// </summary>
		public int PrevYearlyLength { get; set; }
		/// <summary>
		/// 全年发生变化的记录
		/// </summary>
		public virtual IEnumerable<VacationModefyRecord> PrevYealyLengthHistory { get; set; }
		/// <summary>
		/// 年初因上一年度休事假消耗的天数
		/// </summary>
		public int PrevYearlyComsumeLength { get; set; }
	}
	public class VacationModefyRecord
	{
		[Key]
		public int Code { get; set; }
		/// <summary>
		/// 长度
		/// </summary>
		public double Length { get; set; }
		/// <summary>
		/// 生效时间
		/// </summary>
		public DateTime UpdateDate { get; set; }
	}
}


