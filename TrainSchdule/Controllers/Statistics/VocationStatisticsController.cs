using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.Vocations;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
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

		public VocationStatisticsController(ApplicationDbContext context, IGoogleAuthService authService, IUsersService usersService, IUserActionServices userActionServices)
		{
			this.context = context;
			this.authService = authService;
			this.usersService = usersService;
			_userActionServices = userActionServices;
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
		[ProducesResponseType(typeof(VocationStatisticsViewModel), 0)]

		public IActionResult Detail(string statisticsId, string companyCode)
		{
			var cmp = context.Companies.Find(companyCode);
			if (cmp == null) return new JsonResult(ActionStatusMessage.Company.NotExist);
			var statistics = context.VocationStatistics.Find(statisticsId);
			if (statistics == null) return new JsonResult(ActionStatusMessage.Statistics.NotExist);
			var targetCompanyStatistics = context.VocationStatisticsDescriptions.Where<VocationStatisticsDescription>(v => v.StatisticsId == statisticsId && v.Company.Code == companyCode).FirstOrDefault();
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
		[ProducesResponseType(typeof(ApiResult), 0)]
		public async Task<IActionResult> Detail([FromBody]NewStatisticsViewModel model)
		{
			if (!model.Auth.Verify(authService)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var actionUser = usersService.Get(model.Auth.AuthByUserID);
			if (actionUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var targetCompany = context.Companies.Find(model.Data.CompanyCode);
			if (!_userActionServices.Permission(actionUser.Application.Permission,DictionaryAllPermission.Apply.Default, Operation.Query,actionUser.Id, targetCompany.Code)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var baseQuery = new BaseOnTimeVocationStatistics(context, model.Data.Start, model.Data.End, model.Data.StatisticsId)
			{
				CompanyCode = model.Data.CompanyCode??"A",
				Description = model.Data.Description??"用户创建的未知原因查询"
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
			if (!model.Auth.Verify(authService)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var actionUser = usersService.Get(model.Auth.AuthByUserID);
			if (actionUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var targetCompany = context.Companies.Find(model.Data.CompanyCode);
			if (!_userActionServices.Permission(actionUser.Application.Permission,DictionaryAllPermission.Apply.Default, Operation.Remove,actionUser.Id, targetCompany.Code)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var parent = context.VocationStatistics.Find(model.Data.StatisticsId);
			if (parent == null) return new JsonResult(ActionStatusMessage.Statistics.NotExist);

			var entity = context.VocationStatisticsDescriptions.Where(v => v.StatisticsId == model.Data.StatisticsId && v.Company.Code == model.Data.CompanyCode).FirstOrDefault();
			RemoveStatisticsDescription(entity);
			var leftCount = context.VocationStatisticsDescriptions.Count(v => v.StatisticsId == model.Data.StatisticsId);
			if (leftCount == 0) context.VocationStatistics.Remove(parent);
			await context.SaveChangesAsync();
			return new JsonResult(ActionStatusMessage.Success);
		}
		private void RemoveStatisticsDescription(VocationStatisticsDescription entity)
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
			var targetCompanyStatistics = context.VocationStatisticsDescriptions.Where<VocationStatisticsDescription>(v => v.Company.Code == companyCode).ToList();
			bool anyChange = false;
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
					List = targetCompanyStatistics.Select(v => v.ToMode(context.VocationStatistics.Find(v.StatisticsId)))
				}
			});
		}
	}
}