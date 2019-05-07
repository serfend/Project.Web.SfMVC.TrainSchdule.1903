using System.Collections.Generic;
using System.Linq;
using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.Company;
using TrainSchdule.ViewModels.User;

namespace TrainSchdule.Web.Controllers
{
	[Route("[controller]/[action]")]
	public class CompanyController : ControllerBase
	{
		private readonly ICompaniesService _companiesService;
		private readonly ICurrentUserService _currentUserService;
		private readonly IUsersService _usersService;

		public CompanyController(ICompaniesService companiesService, ICurrentUserService currentUserService, IUsersService usersService)
		{
			_companiesService = companiesService;
			_currentUserService = currentUserService;
			_usersService = usersService;
		}
		[HttpGet]
		public IActionResult Child(string id)
		{
			id = id ?? _currentUserService.CurrentUser.CompanyInfo.Company.Code;
			var companys = _companiesService.FindAllChild(id);

			return new JsonResult(new AllChildViewModel()
			{
				Data = new AllChildDataModel()
				{
					List = companys.Select(c => c.ToCompanyModel())
				}
			});
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Managers(string id)
		{
			id = id ?? _currentUserService.CurrentUser.CompanyInfo.Company.Code;
			var list = _companiesService.GetCompanyManagers(id);
			if (list == null) return new JsonResult(ActionStatusMessage.Company.NotExist);
			return new JsonResult(new CompanyManagerViewModel()
			{
				Data = new CompanyManagerDataModel()
				{
					List = list.Select(u => u.ToDTO())
				}
			});
		}

	}
}