﻿using DAL.QueryModel;
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
		public static async Task<Tuple<IQueryable<TSource>, int>> SplitPage<TSource>(this IQueryable<TSource> model, int pageIndex = 0, int pageSize = 20) => await model.SplitPage(new QueryByPage() { PageIndex = pageIndex, PageSize = pageSize }).ConfigureAwait(true);

		public static async Task<Tuple<IQueryable<TSource>, int>> SplitPage<TSource>(this IQueryable<TSource> model, QueryByPage pages)
		{
			if (model == null)
				return null;
			if (pages == null) pages = new QueryByPage()
			{
				PageIndex = 0,
				PageSize = 20
			};
			var totalCount = await Task.Run<int>(() =>
			{
				return model.Count();
			}).ConfigureAwait(true);
			if (pages.PageSize <= 0 || pages.PageIndex < 0) return new Tuple<IQueryable<TSource>, int>(model.Where(f => false), 0);
			var res = pages.PageSize < 0 ? model : model.Skip(pages.PageIndex * pages.PageSize).Take(pages.PageSize);
			return new Tuple<IQueryable<TSource>, int>(res, totalCount);
		}
	}
}