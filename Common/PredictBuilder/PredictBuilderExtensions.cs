using Abp.Linq.Expressions;
using Abp.Specifications;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Common.PredictBuilder
{
	public static class PredictBuilderExtensions
	{
		public static Expression<Func<T, bool>> 
			WhereIf<T,V>(this Expression<Func<T, bool>> expr,V value, Func<V,bool> isWhere, Func<V, Expression<Func<T, bool>>> predict) 
			=> isWhere(value) ? ExpressionFuncExtender.And(expr,predict(value)) : expr;
	}
}