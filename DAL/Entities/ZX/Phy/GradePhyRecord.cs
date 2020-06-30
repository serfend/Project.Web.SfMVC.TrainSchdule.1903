using DAL.Entities.UserInfo;
using DAL.Entities.ZX.Grade;
using System;
using System.Collections.Generic;
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
		public virtual User User { get; set; }

		public virtual User CreateBy { get; set; }
		public DateTime Create { get; set; }
		public virtual GradePhySubject Subject { get; set; }
		public virtual GradeExam Exam { get; set; }
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