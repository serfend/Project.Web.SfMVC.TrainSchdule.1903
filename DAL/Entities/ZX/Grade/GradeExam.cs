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
		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 举办单位
		/// </summary>
		public virtual Company HoldBy { get; set; }

		/// <summary>
		/// 创建人
		/// </summary>

		public virtual User CreateBy { get; set; }

		/// <summary>
		/// 考核处理人
		/// </summary>
		public virtual User HandleBy { get; set; }

		/// <summary>
		/// 举办时间
		/// </summary>
		public DateTime ExecuteTime { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime Create { get; set; }
	}
}