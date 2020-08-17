using DAL.QueryModel;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions.Common
{
	public static class SplitPageExtensions
	{
		public static Tuple<IQueryable<TSource>, int> SplitPage<TSource>(this IQueryable<TSource> model, int pageIndex = 0, int pageSize = 20) => model.SplitPage(new QueryByPage() { PageIndex = pageIndex, PageSize = pageSize });

		public static Tuple<IEnumerable<TSource>, int> SplitPage<TSource>(this IEnumerable<TSource> model, QueryByPage pages)
		{
			if (model == null)
				return null;
			pages = pages.ValidSplitPage();
			var totalCount = model.Count();
			var res = pages.PageSize < 0 || pages.PageIndex < 0 ? model : model.Skip(pages.PageIndex * pages.PageSize).Take(pages.PageSize);
			return new Tuple<IEnumerable<TSource>, int>(res, totalCount);
		}

		public static Tuple<IQueryable<TSource>, int> SplitPage<TSource>(this IQueryable<TSource> model, QueryByPage pages)
		{
			if (model == null)
				return null;
			pages = pages.ValidSplitPage();
			var totalCount = model.Count();
			var res = pages.PageSize < 0 || pages.PageIndex < 0 ? model : model.Skip(pages.PageIndex * pages.PageSize).Take(pages.PageSize);
			return new Tuple<IQueryable<TSource>, int>(res, totalCount);
		}
	}
}