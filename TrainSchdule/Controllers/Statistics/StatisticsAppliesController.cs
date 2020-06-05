using DAL.Entities.Vacations.Statistics.StatisticsNewApply;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Statistics;

namespace TrainSchdule.Controllers.Statistics
{
	public partial class VacationStatisticsController
	{
		/// <summary>
		/// 获取指定单位（包含下级）的新增休假去向统计
		/// </summary>
		/// <param name="companyCode"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public async Task<IActionResult> AppliesTargetNew(string companyCode, DateTime from, DateTime to)
		{
			var r = await Task.Run(() => statisrticsAppliesServices.CaculateNewApplies(companyCode, from, to)).ConfigureAwait(false);
			return new JsonResult(new StatisticsAppliesViewModel<StatisticsApplyNew>()
			{
				Data = new StatisticsAppliesDataModel<StatisticsApplyNew>()
				{
					List = r
				}
			});
		}

		/// <summary>
		/// 获取指定单位（包含下级）的已完成休假去向统计
		/// </summary>
		/// <param name="companyCode"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public async Task<IActionResult> AppliesTargetComplete(string companyCode, DateTime from, DateTime to)
		{
			var r = await Task.Run(() => statisrticsAppliesServices.CaculateCompleteApplies(companyCode, from, to)).ConfigureAwait(false);
			return new JsonResult(new StatisticsAppliesViewModel<StatisticsApplyComplete>()
			{
				Data = new StatisticsAppliesDataModel<StatisticsApplyComplete>()
				{
					List = r
				}
			});
		}
	}
}