using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.ZX.Grade
{
	/// <summary>
	/// 考核
	/// </summary>
	public class GradeExam : BaseEntityInt
	{
		public string Name { get; set; }
		public virtual User CreateBy { get; set; }

		/// <summary>
		/// 考核处理人
		/// </summary>
		public virtual User HandleBy { get; set; }

		public DateTime ExecuteTime { get; set; }
		public DateTime Create { get; set; }
	}
}