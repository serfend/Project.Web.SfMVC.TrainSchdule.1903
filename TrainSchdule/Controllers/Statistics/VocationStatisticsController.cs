using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.Vocations;
using DAL.QueryModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.Crontab;
using TrainSchdule.ViewModels.Statistics;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers.Statistics
{
	/// <summary>
	/// 休假情况统计
	/// </summary>
	[Route("[controller]/[action]")]
	public class VocationStatisticsController : Controller
	{
		private readonly ApplicationDbContext context;
		private readonly IGoogleAuthService authService;
		private readonly IUsersService usersService;
		private readonly IUserActionServices _userActionServices;
		private readonly IVacationStatisticsServices _vacationStatisticsServices;

		/// <summary>
		///
		/// </summary>
		/// <param name="context"></param>
		/// <param name="authService"></param>
		/// <param name="usersService"></param>
		/// <param name="userActionServices"></param>
		/// <param name="vacationStatisticsServices"></param>
		public VocationStatisticsController(ApplicationDbContext context, IGoogleAuthService authService, IUsersService usersService, IUserActionServices userActionServices, IVacationStatisticsServices vacationStatisticsServices)
		{
			this.context = context;
			this.authService = authService;
			this.usersService = usersService;
			_userActionServices = userActionServices;
			_vacationStatisticsServices = vacationStatisticsServices;
		}

		/// <summary>
		/// 获取某日在一年中的周数
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(DateWeekOfYearViewModel), 0)]
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
		public IActionResult DetailsList([FromBody]QueryVacationStatisticsDataModel model)
		{
			var result = _vacationStatisticsServices.Query(model);
			return new JsonResult(new VacationStatisticsDescriptionsViewModel()
			{
				Data = new VacationStatisticsDescriptionsDataModel()
				{
					List = result
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
		public async Task<IActionResult> Detail([FromBody]NewStatisticsViewModel model)
		{
			if (!model.Auth.Verify(authService, null)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var actionUser = usersService.Get(model.Auth.AuthByUserID);
			if (actionUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var targetCompany = context.Companies.Find(model.Data.CompanyCode);
			if (!_userActionServices.Permission(actionUser.Application.Permission, DictionaryAllPermission.Apply.Default, Operation.Query, actionUser.Id, targetCompany.Code)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var baseQuery = new BaseOnTimeVocationStatistics(context, model.Data.Start, model.Data.End, model.Data.StatisticsId)
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
		public async Task<IActionResult> Detail([FromBody]DeleteStatisticsViewModel model)
		{
			if (!model.Auth.Verify(authService, null)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var actionUser = usersService.Get(model.Auth.AuthByUserID);
			if (actionUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var targetCompany = context.Companies.Find(model.Data.CompanyCode);
			if (!_userActionServices.Permission(actionUser.Application.Permission, DictionaryAllPermission.Apply.Default, Operation.Remove, actionUser.Id, targetCompany.Code)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var parent = context.VocationStatistics.Find(model.Data.StatisticsId);
			if (parent == null) return new JsonResult(ActionStatusMessage.Statistics.NotExist);

			var entity = context.VocationStatisticsDescriptions.Where(v => v.StatisticsId == model.Data.StatisticsId && v.Company.Code == model.Data.CompanyCode).FirstOrDefault();
			RemoveStatisticsDescription(entity);
			var leftCount = context.VocationStatisticsDescriptions.Count(v => v.StatisticsId == model.Data.StatisticsId);
			if (leftCount == 0) context.VocationStatistics.Remove(parent);
			await context.SaveChangesAsync();
			return new JsonResult(ActionStatusMessage.Success);
		}

		private void RemoveStatisticsDescription(VacationStatisticsDescription entity)
		{
			if (entity == null) return;
			foreach (var item in entity.Childs) RemoveStatisticsDescription(item);
			var i = entity.IncludeChildLevelStatistics;
			context.VocationStatisticsDescriptionDataStatusCounts.Remove(i.ApplyCount);
			context.VocationStatisticsDescriptionDataStatusCounts.Remove(i.ApplyMembersCount);
			context.VocationStatisticsDescriptionDataStatusCounts.Remove(i.ApplySumDayCount);
			context.VocationStatisticsDatas.Remove(i);

			i = entity.CurrentLevelStatistics;
			context.VocationStatisticsDescriptionDataStatusCounts.Remove(i.ApplyCount);
			context.VocationStatisticsDescriptionDataStatusCounts.Remove(i.ApplyMembersCount);
			context.VocationStatisticsDescriptionDataStatusCounts.Remove(i.ApplySumDayCount);
			context.VocationStatisticsDatas.Remove(i);

			context.VocationStatisticsDescriptions.Remove(entity);
		}

		/// <summary>
		/// 单位统计记录
		/// </summary>
		/// <param name="companyCode"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Summary(string companyCode)
		{
			var cmp = context.Companies.Find(companyCode);
			if (cmp == null) return new JsonResult(ActionStatusMessage.Company.NotExist);
			var targetCompanyStatistics = context.VocationStatisticsDescriptions.Where<VacationStatisticsDescription>(v => v.Company.Code == companyCode).OrderBy(v => v.StatisticsId).ToList();
			bool anyChange = false;// 当本级不存在时，删除本级
			foreach (var item in targetCompanyStatistics) if (item.StatisticsId == null)
				{
					anyChange = true;
					RemoveStatisticsDescription(item);
				}
			if (anyChange)
			{
				foreach (var vs in context.VocationStatistics)
				{
					var leftCount = context.VocationStatisticsDescriptions.Count(v => v.StatisticsId == vs.Id);
					if (leftCount == 0) context.VocationStatistics.Remove(vs);
				}
				context.SaveChanges();
			}
			return new JsonResult(new NewStatisticsListViewModel()
			{
				Data = new NewStatisticsListDataModel()
				{
					List = targetCompanyStatistics.Select(v => v.ToSummaryModel(context.VocationStatistics.Find(v.StatisticsId)))
				}
			});
		}
	}
}