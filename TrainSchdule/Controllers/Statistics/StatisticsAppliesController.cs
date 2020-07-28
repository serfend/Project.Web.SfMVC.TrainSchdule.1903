using BLL.Helpers;
using DAL.Entities.UserInfo;
using DAL.Entities.Vacations.Statistics.StatisticsNewApply;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
		/// 重新加载所有统计记录
		/// </summary>
		/// <returns></returns>
		[RequireHttps]
		[HttpPost]
		public async Task<IActionResult> ReloadAllStatistics(DateTime from, DateTime to)
		{
			var ua = _userActionServices.Log(UserOperation.FromSystemReport, "#System#", "更新统计情况", false);
			if (HttpContext?.Connection?.RemoteIpAddress != HttpContext?.Connection?.LocalIpAddress) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var removeActions = new Task[]
			{
				new Task(() => statisticsAppliesServices.RemoveCompleteApplies("", from, to)),
				new Task(() => statisticsAppliesServices.RemoveNewApplies("", from, to)),
				new Task(() => statisticsAppliesProcessServices.RemoveCompleteApplies("", from, to)),
				new Task(() => statisticsDailyProcessServices.RemoveCompleteApplies("", from, to))
			};
			foreach (var t in removeActions)
			{
				t.Start();
				await t.ConfigureAwait(true);
			}
			_userActionServices.Status(ua, false, "删除原记录");
			var allCompanies = context.Companies.Select(c => c.Code).ToList();
			_userActionServices.Status(ua, false, JsonConvert.SerializeObject(new Tuple<DateTime, string>(DateTime.Now, $"重建{allCompanies.Count}个单位的记录")));
			var reloadActions = new List<Task>();
			int total = allCompanies.Count * 4;
			int current = 0;
			foreach (var c in allCompanies)
			{
				reloadActions.Add(new Task(() => statisticsAppliesServices.CaculateCompleteApplies(c, from, to)));
				reloadActions.Add(new Task(() => statisticsAppliesServices.CaculateNewApplies(c, from, to)));
				reloadActions.Add(new Task(() => statisticsAppliesProcessServices.CaculateCompleteApplies(c, from, to)));
				reloadActions.Add(new Task(() => statisticsDailyProcessServices.CaculateCompleteApplies(c, from, to)));
				if (reloadActions.Count > 8)
				{
					foreach (var t in reloadActions)
					{
						t.Start();
						await t.ConfigureAwait(true);
					}
					current += reloadActions.Count;
					_userActionServices.Status(ua, false, JsonConvert.SerializeObject(new Tuple<DateTime, string>(DateTime.Now, $"{Math.Round((100 * current / (decimal)total), 2)}% ")));
					reloadActions.Clear();
				}
			}
			_userActionServices.Status(ua, true, JsonConvert.SerializeObject(new Tuple<DateTime, string>(DateTime.Now, "完成重新加载")));
			return new JsonResult(ActionStatusMessage.Success);
		}

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
			var result = await StatisticsResultExtensions.GetTarget(statisticsAppliesServices.DirectGetNewApplies, companyCode, from, to).ConfigureAwait(false);
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
			var result = await StatisticsResultExtensions.GetTarget(statisticsAppliesServices.DirectGetCompleteApplies, companyCode, from, to);
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
			await Task.Run(() => { statisticsAppliesServices.RemoveNewApplies(companyCode, from, to); }).ConfigureAwait(false);
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
			await Task.Run(() => { statisticsAppliesServices.RemoveCompleteApplies(companyCode, from, to); }).ConfigureAwait(false);
			return new JsonResult(ActionStatusMessage.Success);
		}
	}
}