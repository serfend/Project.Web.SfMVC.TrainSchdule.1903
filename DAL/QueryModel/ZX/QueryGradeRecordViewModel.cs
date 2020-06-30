using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.QueryModel.ZX
{
	public class QueryGradeRecordViewModel
	{
		public QueryByDate Create { get; set; }
		public QueryByString CreateBy { get; set; }
		public QueryByString CreateFor { get; set; }
		public QueryByPage Pages { get; set; }
	}
}