using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.Vacations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Interfaces.IVacationStatistics
{
	/// <summary>
	/// TODO 用于规范各统计的GetTargetStatistics方法
	/// </summary>
	public interface IStatisticsCaculate
	{
		Tuple<IEnumerable<T>, bool> GetTargetStatistics<T>(string companyCode, DateTime target, IQueryable<T> db, IQueryable<Apply> applies, IQueryable<RecallOrder> recallDb) where T : class;
	}

	public interface IStatisticsNew<T>
	{
		/// <summary>
		/// 获取单位在指定时间内的新增休假情况
		/// </summary>
		/// <param name="companyCode"></param>
		/// <param name="vStart"></param>
		/// <param name="vEnd"></param>
		/// <returns></returns>
		IEnumerable<T> CaculateNewApplies(string companyCode, DateTime vStart, DateTime vEnd);

		/// <summary>
		/// 删除单位在指定时间内的新增休假情况
		/// </summary>
		/// <param name="companyCode"></param>
		/// <param name="vStart"></param>
		/// <param name="vEnd"></param>
		/// <returns></returns>
		void RemoveNewApplies(string companyCode, DateTime vStart, DateTime vEnd);
	}

	public interface IStatisticsComplete<T>
	{
		/// <summary>
		/// 获取单位在指定时间内的完成休假情况
		/// </summary>
		/// <param name="companyCode"></param>
		/// <param name="vStart"></param>
		/// <param name="vEnd"></param>
		/// <returns></returns>
		IEnumerable<T> CaculateCompleteApplies(string companyCode, DateTime vStart, DateTime vEnd);

		/// <summary>
		/// 删除单位在指定时间内的完成休假情况
		/// </summary>
		/// <param name="companyCode"></param>
		/// <param name="vStart"></param>
		/// <param name="vEnd"></param>
		/// <returns></returns>
		void RemoveCompleteApplies(string companyCode, DateTime vStart, DateTime vEnd);
	}
}