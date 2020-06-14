using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.Extensions.StatisticsExtensions
{
	/// <summary>
	/// 统计结果获取
	/// </summary>
	public static class StatisticsResultExtensions
	{
		/// <summary>
		/// 通过方法来获取统计结果
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="GetResultMethod"></param>
		/// <param name="companyCode"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static async Task<IEnumerable<EntitiesListDataModel<T>>> GetTarget<T>(this Func<string, DateTime, DateTime, IEnumerable<T>> GetResultMethod, string companyCode, DateTime from, DateTime to)
		{
			var companiesCode = companyCode.Split("##");
			var result = new List<EntitiesListDataModel<T>>();
			foreach (var c in companiesCode)
			{
				var r = await Task.Run(() => GetResultMethod(c, from, to)).ConfigureAwait(false);
				result.Add(new EntitiesListDataModel<T>(r));
			}
			return result;
		}
	}
}