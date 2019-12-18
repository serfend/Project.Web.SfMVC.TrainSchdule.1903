using System.Collections.Generic;
using System.Linq;
using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
		private readonly ICompanyManagerServices _companyManagerServices;
		private readonly IUserServiceDetail _usersService;
		private readonly IHostingEnvironment _hostingEnvironment;
		/// <summary>
		/// 单位信息
		/// </summary>
		/// <param name="companiesService"></param>
		/// <param name="currentUserService"></param>
		/// <param name="companyManagerServices"></param>
		/// <param name="usersService"></param>
		public CompanyController(ICompaniesService companiesService, ICurrentUserService currentUserService, ICompanyManagerServices companyManagerServices, IUserServiceDetail usersService, IHostingEnvironment hostingEnvironment)
		{
			_companiesService = companiesService;
			_currentUserService = currentUserService;
			_companyManagerServices = companyManagerServices;
			_usersService = usersService;
			_hostingEnvironment = hostingEnvironment;
		}

		/// <summary>
		/// 获取单位的子层级单位
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Child(string id)
		{
			var currentUser = _currentUserService.CurrentUser;
			id = id ?? currentUser?.CompanyInfo?.Company?.Code;
			List<Company> list ;
			list=  _companiesService.FindAllChild(id).ToList();
			if (currentUser != null)
			{
				var mymanage = _usersService.InMyManage(currentUser.Id, out var totalCount);
				list.AddRange(mymanage);
			}
			return new JsonResult(new AllChildViewModel()
			{
				Data = new AllChildDataModel()
				{
					List = list.Select(c => c.ToCompanyModel())
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
					List = list.Select(u => u.ToSummaryDto(_hostingEnvironment))
				}
			});
		}
		/// <summary>
		/// 获取单位中的人员列表
		/// </summary>
		/// <param name="code"></param>
		/// <param name="page"></param>
		/// <param name="pageSize">默认每页为100个用户</param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(AllMembersDataModel), 0)]

		public IActionResult Members(string code, int page, int pageSize = 100)
		{
			code = code ?? _currentUserService.CurrentUser?.CompanyInfo.Company?.Code;
			var list = _companyManagerServices.GetMembers(code, page, pageSize,out var totalCount).Select(u => u.ToSummaryDto(_hostingEnvironment));
			return new JsonResult(new AllMembersViewModel()
			{
				Data = new AllMembersDataModel()
				{
					List = list,TotalCount= totalCount
				}
			});
		}
	}
}