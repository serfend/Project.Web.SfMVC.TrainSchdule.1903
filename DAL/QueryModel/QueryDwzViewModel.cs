using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.QueryModel
{
	public class QueryDwzViewModel
	{
		public QueryByString Key { get; set; }
		public QueryByString Target { get; set; }
		public QueryByString CreateBy { get; set; }
		public QueryByPage Pages { get; set; }
		public QueryByDate Create { get; set; }
		public QueryByString Ip { get; set; }
		public QueryByString Device { get; set; }
	}

	public class QueryDwzStatisticsViewModel
	{
		public QueryByDate Create { get; set; }
		public QueryByString ViewBy { get; set; }
		public QueryByString Ip { get; set; }
		public QueryByString Device { get; set; }
		public QueryByPage Pages { get; set; }
	}
}