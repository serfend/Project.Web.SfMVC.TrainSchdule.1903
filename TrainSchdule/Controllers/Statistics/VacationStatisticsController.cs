using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.IVacationStatistics;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.Vacations;
using DAL.Entities.Vacations.Statistics;
using DAL.QueryModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.Crontab;
using TrainSchdule.Extensions.StatisticsExtensions;
using TrainSchdule.ViewModels.Statistics;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers.Statistics
{
	/// <summary>
	/// 休假情况统计
	/// </summary>
	[Route("[controller]")]
	public partial class VacationStatisticsController : Controller
	{
		private readonly ApplicationDbContext context;
		private readonly IGoogleAuthService authService;
		private readonly IUsersService usersService;
		private readonly IUserActionServices _userActionServices;
		private readonly ICompaniesService companiesService;
		private readonly IStatisrticsAppliesServices statisticsAppliesServices;
		private readonly IStatisticsAppliesProcessServices statisticsAppliesProcessServices;
		private readonly IStatisticsDailyProcessServices statisticsDailyProcessServices;

		/// <summary>
		///
		/// </summary>
		/// <param name="context"></param>
		/// <param name="authService"></param>
		/// <param name="usersService"></param>
		/// <param name="userActionServices"></param>
		/// <param name="companiesService"></param>
		/// <param name="statisrticsAppliesServices"></param>
		/// <param name="statisticsAppliesProcessServices"></param>
		/// <param name="statisticsDailyProcessServices"></param>
		public VacationStatisticsController(ApplicationDbContext context, IGoogleAuthService authService, IUsersService usersService, IUserActionServices userActionServices, ICompaniesService companiesService, IStatisrticsAppliesServices statisrticsAppliesServices, IStatisticsAppliesProcessServices statisticsAppliesProcessServices, IStatisticsDailyProcessServices statisticsDailyProcessServices)
		{
			this.context = context;
			this.authService = authService;
			this.usersService = usersService;
			_userActionServices = userActionServices;
			this.companiesService = companiesService;
			this.statisticsAppliesServices = statisrticsAppliesServices;
			this.statisticsAppliesProcessServices = statisticsAppliesProcessServices;
			this.statisticsDailyProcessServices = statisticsDailyProcessServices;
		}

		/// <summary>
		/// 获取某日在一年中的周数
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(DateWeekOfYearViewModel), 0)]
		[Route("WhichWeekInYear")]
		public IActionResult WhichWeekInYear(DateTime value)
		{
			var result = new GregorianCalendar().GetWeekOfYear(value, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday) + 1;
			return new JsonResult(new DateWeekOfYearViewModel()
			{
				Data = new DateWeekOfYearDataModel() { WeekOfYear = result }
			});
		}

		/// <summary>
		/// 获取指定单位指定时间内的休假情况
		/// </summary>
		/// <param name="companyCode"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("AppliesProcessRecord")]
		public async Task<IActionResult> GetAppliesProcess(string companyCode, DateTime from, DateTime to)
		{
			var result = await StatisticsResultExtensions.GetTarget(statisticsAppliesProcessServices.DirectGetCompleteApplies, companyCode, from, to).ConfigureAwait(false);
			var r = new EntitiesListViewModel<EntitiesListDataModel<StatisticsAppliesProcess>>(result);
			return new JsonResult(r);
		}

		/// <summary>
		/// 删除指定单位指定时间内的休假情况
		/// </summary>
		/// <param name="companyCode"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("AppliesProcessRecord")]
		public async Task<IActionResult> RemoveAppliesProcess(string companyCode, DateTime from, DateTime to)
		{
			await Task.Run(() => { statisticsAppliesProcessServices.RemoveCompleteApplies(companyCode, from, to); }).ConfigureAwait(false);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 获取指定单位指定时间内需累积项的休假情况
		/// </summary>
		/// <param name="companyCode"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("AppliesDailyProcessRecord")]
		public async Task<IActionResult> GetAppliesDailyProcess(string companyCode, DateTime from, DateTime to)
		{
			var result = await StatisticsResultExtensions.GetTarget(statisticsDailyProcessServices.DirectGetCompleteApplies, companyCode, from, to).ConfigureAwait(false);
			return new JsonResult(new EntitiesListViewModel<EntitiesListDataModel<StatisticsDailyProcessRate>>(result));
		}

		/// <summary>
		/// 删除指定单位指定时间内需累积项的休假情况
		/// </summary>
		/// <param name="companyCode"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("AppliesDailyProcessRecord")]
		public async Task<IActionResult> RemoveAppliesDailyProcess(string companyCode, DateTime from, DateTime to)
		{
			await Task.Run(() => { statisticsDailyProcessServices.RemoveCompleteApplies(companyCode, from, to); }).ConfigureAwait(false);
			return new JsonResult(ActionStatusMessage.Success);
		}
	}
}