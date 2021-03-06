using DAL.Entities.UserInfo;
using DAL.Entities.ZX.Grade;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entities.ZX.Phy
{
	/// <summary>
	/// 成绩记录
	/// </summary>
	public class GradePhyRecord : BaseEntityInt
	{
		/// <summary>
		/// 所属人
		/// </summary>
		[ForeignKey("UserId")]
		public virtual User User { get; set; }
		public string UserId { get; set; }

		/// <summary>
		/// 创建人
		/// </summary>
		[ForeignKey("CreateById")]
		public virtual User CreateBy { get; set; }
		public string CreateById { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime Create { get; set; }

		/// <summary>
		/// 科目
		/// </summary>
		public virtual GradePhySubject Subject { get; set; }

		/// <summary>
		/// 考核
		/// </summary>
		public virtual GradeExam Exam { get; set; }

		/// <summary>
		/// 成绩
		/// </summary>
		public int Score { get; set; }

		/// <summary>
		/// 成绩原始值
		/// </summary>
		public string RawValue { get; set; }

		/// <summary>
		/// 用户备注标识
		/// </summary>
		public string Remark { get; set; }
	}
}