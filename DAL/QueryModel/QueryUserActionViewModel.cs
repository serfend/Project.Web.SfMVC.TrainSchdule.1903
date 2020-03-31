using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.QueryModel
{
	public class QueryUserActionViewModel
	{
		public QueryByString UserName { get; set; }
		public QueryByDate Date { get; set; }
		public QueryByIntOrEnum Rank { get; set; }

		public QueryByString Ip { get; set; }
		public QueryByString Device { get; set; }
		public QueryByString Message { get; set; }

		public QueryByPage Page { get; set; }
	}
}