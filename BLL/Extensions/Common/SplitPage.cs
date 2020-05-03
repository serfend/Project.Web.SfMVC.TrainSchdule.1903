using DAL.QueryModel;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Extensions.Common
{
	public static class SplitPageExtensions
	{
		public static IQueryable<TSource> SplitPage<TSource>(this IQueryable<TSource> model, QueryByPage pages, out int totalCount)
		{
			if (model == null)
			{
				totalCount = 0;
				return null;
			}
			if (pages == null) pages = new QueryByPage()
			{
				PageIndex = 0,
				PageSize = 20
			};
			totalCount = model.Count();
			if (pages.PageSize <= 0 || pages.PageIndex < 0) return model.Where(f => false);
			var res = model.Skip(pages.PageIndex * pages.PageSize).Take(pages.PageSize);
			return res;
		}
	}
}