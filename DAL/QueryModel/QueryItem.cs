using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.QueryModel
{
	/// <summary>
	/// 通过整型或者枚举来查询（枚举将会被转换为整型）
	/// </summary>
	public class QueryByIntOrEnum
	{
		/// <summary>
		/// 最小值（包含）
		/// </summary>
		public int? Start { get; set; }
		/// <summary>
		/// 最大值（包含）
		/// </summary>
		public int? End { get; set; }
		/// <summary>
		/// 查询个别值
		/// </summary>
		public IEnumerable<int> Arrays { get; set; }
	}
	/// <summary>
	/// 通过页面查询
	/// </summary>
	public class QueryByPage
	{
		/// <summary>
		/// 页面号，从0开始
		/// </summary>
		public int PageIndex { get; set; }
		/// <summary>
		/// 每页的条数
		/// </summary>
		public int PageSize { get; set; }
	}
	/// <summary>
	/// 通过日期来查询
	/// </summary>
	public class QueryByDate
	{
		/// <summary>
		/// 日期开始时间（包含）
		/// </summary>
		public DateTime Start { get; set; }
		/// <summary>
		/// 日期结束时间（包含）
		/// </summary>
		public DateTime End { get; set; }
		/// <summary>
		/// 查询个别日期
		/// </summary>
		public IEnumerable<DateTime> Dates { get; set; }
 }
	/// <summary>
	/// 通过字符串查询（
	/// </summary>
	public class QueryByString
	{
		public string Value { get; set; }
	}
	public class QueryByGuid
	{
		public IEnumerable<Guid> Arrays { get; set; }
	}
}
