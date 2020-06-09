using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.IVacationStatistics;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.Vacations;
using DAL.Entities.Vacations.VacationsStatistics;
using DAL.QueryModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.Crontab;
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
		private readonly IVacationStatisticsServices _vacationStatisticsServices;
		private readonly ICompaniesService companiesService;
		private readonly IStatisrticsAppliesServices statisrticsAppliesServices;

		/// <summary>
		///
		/// </summary>
		/// <param name="context"></param>
		/// <param name="authService"></param>
		/// <param name="usersService"></param>
		/// <param name="userActionServices"></param>
		/// <param name="vacationStatisticsServices"></param>
		/// <param name="companiesService"></param>
		/// <param name="statisrticsAppliesServices"></param>
		public VacationStatisticsController(ApplicationDbContext context, IGoogleAuthService authService, IUsersService usersService, IUserActionServices userActionServices, IVacationStatisticsServices vacationStatisticsServices, ICompaniesService companiesService, IStatisrticsAppliesServices statisrticsAppliesServices)
		{
			this.context = context;
			this.authService = authService;
			this.usersService = usersService;
			_userActionServices = userActionServices;
			_vacationStatisticsServices = vacationStatisticsServices;
			this.companiesService = companiesService;
			this.statisrticsAppliesServices = statisrticsAppliesServices;
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
		/// 获取指定统计内某单位的情况
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(typeof(VacationStatisticsViewModel), 0)]
		[Route("DetailsList")]
		public IActionResult DetailsList([FromBody]QueryVacationStatisticsViewModel model)
		{
			var result = _vacationStatisticsServices.Query(model);
			return new JsonResult(new VacationStatisticsDescriptionsViewModel()
			{
				Data = new EntitiesListDataModel<VacationStatisticsDescription>()
				{
					List = result.Item1,
					TotalCount = result.Item2
				}
			});
		}

		/// <summary>
		/// 添加新的统计任务
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(typeof(ApiResult), 0)]
		[Route("Detail")]
		public async Task<IActionResult> Detail([FromBody]NewStatisticsViewModel model)
		{
			if (!model.Auth.Verify(authService, null)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var actionUser = usersService.Get(model.Auth.AuthByUserID);
			if (actionUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
			var targetCompany = context.Companies.Find(model.Data.CompanyCode);
			if (!_userActionServices.Permission(actionUser.Application.Permission, DictionaryAllPermission.Apply.Default, Operation.Query, actionUser.Id, targetCompany.Code)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var baseQuery = new BaseOnTimeVacationStatistics(context, companiesService, model.Data.Start, model.Data.End, model.Data.StatisticsId)
			{
				CompanyCode = model.Data.CompanyCode ?? "A",
				Description = model.Data.Description ?? "用户创建的未知原因查询"
			};

			try
			{
				await baseQuery.RunAsync();
			}
			catch (ActionStatusMessageException ex)
			{
				return new JsonResult(ex.Status);
			}
			catch (Exception ex)
			{
				return new JsonResult(new ApiResult(-1, ex.Message));
			}
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 删除指定单位的指定统计
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpDelete]
		[ProducesResponseType(typeof(ApiResult), 0)]
		[Route("Detail")]
		public async Task<IActionResult> Detail([FromBody]DeleteStatisticsViewModel model)
		{
			if (!model.Auth.Verify(authService, null)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var actionUser = usersService.Get(model.Auth.AuthByUserID);
			if (actionUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
			var targetCompany = context.Companies.Find(model.Data.CompanyCode);
			if (!_userActionServices.Permission(actionUser.Application.Permission, DictionaryAllPermission.Apply.Default, Operation.Remove, actionUser.Id, targetCompany.Code)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var parent = context.VacationStatistics.Find(model.Data.StatisticsId);
			if (parent == null) return new JsonResult(ActionStatusMessage.Statistics.NotExist);

			var entity = context.VacationStatisticsDescriptions.Where(v => v.StatisticsId == model.Data.StatisticsId && v.Company.Code == model.Data.CompanyCode).FirstOrDefault();
			RemoveStatisticsDescription(entity);
			var leftCount = context.VacationStatisticsDescriptions.Count(v => v.StatisticsId == model.Data.StatisticsId);
			if (leftCount == 0) context.VacationStatistics.Remove(parent);
			await context.SaveChangesAsync();
			return new JsonResult(ActionStatusMessage.Success);
		}

		private void RemoveStatisticsDescription(VacationStatisticsDescription entity)
		{
			if (entity == null) return;
			foreach (var item in entity.Childs) RemoveStatisticsDescription(item);
			var i = entity.Data;
			context.VacationStatisticsDatas.RemoveRange(i);
			context.VacationStatisticsDescriptions.Remove(entity);
		}

		/// <summary>
		/// 单位统计记录
		/// </summary>
		/// <param name="companiesCode">需要查询的单位代码，以##分割</param>
		/// <returns></returns>
		[HttpGet]
		[Route("Summary")]
		public IActionResult Summary(string companiesCode)
		{
			var companies = companiesCode?.Split("##");
			if (companies == null || companies.Length == 0) return new JsonResult(ActionStatusMessage.CompanyMessage.NotExist);
			var statisticsIds = context.VacationStatisticsDescriptions.
				Where<VacationStatisticsDescription>(v => companies.Contains(v.Company.Code))
				.Select(v => v.StatisticsId).Distinct()
				.ToList();
			var list = statisticsIds.Select(v => new NewStatisticsSingleDataModel(v, context.VacationStatistics.Where(s => s.Id == v).FirstOrDefault()));
			return new JsonResult(new NewStatisticsListViewModel()
			{
				Data = new EntitiesListDataModel<NewStatisticsSingleDataModel>()
				{
					List = list,
					TotalCount = list.Count()
				}
			});
		}
	}
}