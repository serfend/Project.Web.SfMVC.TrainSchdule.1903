using System;
using System.Collections.Generic;
using System.Linq;

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
		public QueryByPage()
        {
			PageIndex = 0;
			PageSize = 20;
        }
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
		public IEnumerable<string> Arrays { get; set; }
	}

	public class QueryByGuid
	{
		public IEnumerable<Guid> Arrays { get; set; }
		public Guid Value { get; set; }
	}

	public static class QueryModelValid
	{
		public static bool Valid(this QueryByDate model)
		{
			if (model == null) return false;
			var dates = model.Dates?.Any() ?? false;
			var range = model.Start != DateTime.MinValue || model.End >= model.Start;
			return range || dates;
		}

		public static bool Valid(this QueryByString model)
		{
			if (model == null) return false;
			var v = model.Value != null || (model.Arrays?.Any() ?? false);
			return v;
		}

		public static bool Valid(this QueryByIntOrEnum model)
		{
			if (model == null) return false;
			return model.End >= model.Start || (model.Arrays?.Any() ?? false);
		}

		public static QueryByPage ValidSplitPage(this QueryByPage model, int pageIndex = 0, int pageSize = 20)
		{
			if (model == null) model = new QueryByPage() { PageIndex = pageIndex, PageSize = pageSize };
			return model;
		}
	}
}