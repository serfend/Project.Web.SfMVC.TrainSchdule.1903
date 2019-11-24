using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities.Vocations;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.Crontab;
using TrainSchdule.ViewModels.Statistics;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers.Statistics
{
	public class VocationStatisticsController : Controller
	{
		private readonly ApplicationDbContext context;
		private readonly IGoogleAuthService authService;

		public VocationStatisticsController(ApplicationDbContext context, IGoogleAuthService authService)
		{
			this.context = context;
			this.authService = authService;
		}

		/// <summary>
		/// 获取某日在一年中的周数
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(DateWeekOfYearViewModel), 0)]
		public IActionResult WhichWeekInYear(DateTime date)
		{
			var result = new GregorianCalendar().GetWeekOfYear(date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday) + 1;
			return new JsonResult(new DateWeekOfYearViewModel()
			{
				Data = new DateWeekOfYearDataModel() { WeekOfYear = result }
			});
		}
		/// <summary>
		/// 获取指定统计内某单位的情况
		/// </summary>
		/// <param name="statisticsId"></param>
		/// <param name="companyCode"></param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(VocationStatisticsViewModel),0)]

		public IActionResult FromCompany(string statisticsId, string companyCode)
		{
			var cmp = context.Companies.Find(companyCode);
			if (cmp == null) return new JsonResult(ActionStatusMessage.Company.NotExist);
			var statistics = context.VocationStatistics.Find(statisticsId);
			if (statistics == null) return new JsonResult(ActionStatusMessage.Statistics.NotExist);
			var targetCompanyStatistics = context.VocationStatisticsDescriptions.Where<VocationStatisticsDescription>(v => v.StatisticsId == statisticsId&&v.Company.Code==companyCode).FirstOrDefault();
			return new JsonResult(new VocationStatisticsViewModel()
			{
				Data = targetCompanyStatistics
			});
		}
		/// <summary>
		/// 添加新的统计任务
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(typeof(Status), 0)]
		public IActionResult NewStatistics([FromBody]NewStatisticsViewModel model)
		{
			if (!model.Auth.Verify(authService)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);

			var baseQuery = new BaseOnTimeVocationStatistics(context, model.Data.Start, model.Data.End, model.Data.StatisticsId);
			baseQuery.CompanyCode = model.Data.CompanyCode;
			BackgroundJob.Enqueue(() =>baseQuery.Run());
			return new JsonResult(ActionStatusMessage.Success);
		}
	}
}