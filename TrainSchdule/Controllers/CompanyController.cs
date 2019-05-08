using System.Linq;
using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.Company;

namespace TrainSchdule.Controllers
{
	/// <summary>
	/// 单位管理
	/// </summary>
	[Route("[controller]/[action]")]
	public class CompanyController : ControllerBase
	{
		private readonly ICompaniesService _companiesService;
		private readonly ICurrentUserService _currentUserService;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="companiesService"></param>
		/// <param name="currentUserService"></param>
		/// <param name="usersService"></param>
		public CompanyController(ICompaniesService companiesService, ICurrentUserService currentUserService, IUsersService usersService)
		{
			_companiesService = companiesService;
			_currentUserService = currentUserService;
		}
		/// <summary>
		/// 获取单位的子层级单位
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Child(string id)
		{
			id = id ?? _currentUserService.CurrentUser.CompanyInfo.Company.Code;
			var company = _companiesService.FindAllChild(id);

			return new JsonResult(new AllChildViewModel()
			{
				Data = new AllChildDataModel()
				{
					List = company.Select(c => c.ToCompanyModel())
				}
			});
		}

		/// <summary>
		/// 获取单位的主管
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(CompanyManagerDataModel), 0)]

		public IActionResult Managers(string id)
		{
			id = id ?? _currentUserService.CurrentUser?.CompanyInfo?.Company?.Code;
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