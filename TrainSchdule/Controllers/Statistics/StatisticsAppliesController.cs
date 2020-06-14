using BLL.Helpers;
using DAL.Entities.Vacations.Statistics.StatisticsNewApply;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.Extensions.StatisticsExtensions;
using TrainSchdule.ViewModels.Statistics;
using TrainSchdule.ViewModels.System;

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
		[HttpGet]
		[Route("AppliesTargetNew")]
		public async Task<IActionResult> GetAppliesTargetNew(string companyCode, DateTime from, DateTime to)
		{
			var result = await StatisticsResultExtensions.GetTarget(statisrticsAppliesServices.CaculateNewApplies, companyCode, from, to).ConfigureAwait(false);
			return new JsonResult(new EntitiesListViewModel<EntitiesListDataModel<StatisticsApplyNew>>(result));
		}

		/// <summary>
		/// 获取指定单位（包含下级）的已完成休假去向统计
		/// </summary>
		/// <param name="companyCode"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("AppliesTargetComplete")]
		public async Task<IActionResult> GetAppliesTargetComplete(string companyCode, DateTime from, DateTime to)
		{
			var result = await StatisticsResultExtensions.GetTarget(statisrticsAppliesServices.CaculateCompleteApplies, companyCode, from, to);
			return new JsonResult(new EntitiesListViewModel<EntitiesListDataModel<StatisticsApplyComplete>>(result));
		}

		/// <summary>
		/// 删除指定单位（包含下级）的新增休假去向统计
		/// </summary>
		/// <param name="companyCode"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("AppliesTargetNew")]
		public async Task<IActionResult> RemoveAppliesTargetNew(string companyCode, DateTime from, DateTime to)
		{
			await Task.Run(() => { statisrticsAppliesServices.RemoveNewApplies(companyCode, from, to); }).ConfigureAwait(false);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 删除指定单位（包含下级）的已完成休假去向统计
		/// </summary>
		/// <param name="companyCode"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("AppliesTargetComplete")]
		public async Task<IActionResult> RemoveAppliesTargetComplete(string companyCode, DateTime from, DateTime to)
		{
			await Task.Run(() => { statisrticsAppliesServices.RemoveCompleteApplies(companyCode, from, to); }).ConfigureAwait(false);
			return new JsonResult(ActionStatusMessage.Success);
		}
	}
}