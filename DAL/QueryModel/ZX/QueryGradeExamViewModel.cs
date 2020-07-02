using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.QueryModel.ZX
{
	public class QueryGradeExamViewModel
	{
		public QueryByPage Pages { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		public QueryByString Name { get; set; }

		/// <summary>
		/// 举办单位
		/// </summary>
		public QueryByString HoldBy { get; set; }

		/// <summary>
		/// 创建人
		/// </summary>

		public QueryByString CreateBy { get; set; }

		/// <summary>
		/// 考核处理人
		/// </summary>
		public QueryByString HandleBy { get; set; }

		/// <summary>
		/// 举办时间
		/// </summary>
		public QueryByDate ExecuteTime { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public QueryByDate Create { get; set; }
	}
}