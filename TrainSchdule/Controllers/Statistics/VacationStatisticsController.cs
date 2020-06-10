using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.IVacationStatistics;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.Vacations;
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
		public VacationStatisticsController(ApplicationDbContext context, IGoogleAuthService authService, IUsersService usersService, IUserActionServices userActionServices, ICompaniesService companiesService, IStatisrticsAppliesServices statisrticsAppliesServices)
		{
			this.context = context;
			this.authService = authService;
			this.usersService = usersService;
			_userActionServices = userActionServices;
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
	}
}