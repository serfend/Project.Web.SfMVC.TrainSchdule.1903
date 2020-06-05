using DAL.Entities.Vacations.Statistics.StatisticsNewApply;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces.IVacationStatistics
{
	public interface IStatisrticsAppliesServices
	{
		/// <summary>
		/// 获取单位在指定时间内的新增休假情况
		/// </summary>
		/// <param name="companyCode"></param>
		/// <param name="vStart"></param>
		/// <param name="vEnd"></param>
		/// <returns></returns>
		IEnumerable<StatisticsApplyNew> CaculateNewApplies(string companyCode, DateTime vStart, DateTime vEnd);

		/// <summary>
		/// 获取单位在指定时间内的完成休假情况
		/// </summary>
		/// <param name="companyCode"></param>
		/// <param name="vStart"></param>
		/// <param name="vEnd"></param>
		/// <returns></returns>
		IEnumerable<StatisticsApplyComplete> CaculateCompleteApplies(string companyCode, DateTime vStart, DateTime vEnd);
	}
}