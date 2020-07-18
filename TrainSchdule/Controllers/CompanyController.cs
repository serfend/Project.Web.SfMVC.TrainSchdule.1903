using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Extensions;
using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using Castle.Core.Internal;
using DAL.Data;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.Company;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.Controllers
{
	/// <summary>
	/// 单位管理
	/// </summary>
	[Route("[controller]/[action]")]
	public class CompanyController : Controller
	{
		private readonly ICompaniesService _companiesService;
		private readonly ICurrentUserService _currentUserService;
		private readonly ICompanyManagerServices _companyManagerServices;
		private readonly IUserServiceDetail _usersService;
		private readonly ApplicationDbContext _context;

		/// <summary>
		/// 单位信息
		/// </summary>
		/// <param name="companiesService"></param>
		/// <param name="currentUserService"></param>
		/// <param name="companyManagerServices"></param>
		/// <param name="usersService"></param>
		/// <param name="context"></param>
		public CompanyController(ICompaniesService companiesService, ICurrentUserService currentUserService, ICompanyManagerServices companyManagerServices, IUserServiceDetail usersService, ApplicationDbContext context)
		{
			_companiesService = companiesService;
			_currentUserService = currentUserService;
			_companyManagerServices = companyManagerServices;
			_usersService = usersService;
			_context = context;
		}

		/// <summary>
		/// 获取指定岗位名称的详细信息
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult DutiesDetail(string name)
		{
			var currentUser = _currentUserService.CurrentUser;
			name = name ?? currentUser?.CompanyInfo?.Duties.Name;
			var duty = _context.Duties.Where(d => d.Name == name).FirstOrDefault();
			if (duty == null) return new JsonResult(ActionStatusMessage.CompanyMessage.DutyMessage.NotExist);
			var r = duty.ToDataModel();
			return new JsonResult(new DutyViewModel()
			{
				Data = r
			});
		}

		/// <summary>
		/// 获取指定名称可能的岗位
		/// </summary>
		/// <param name="name"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageNum"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult DutiesQuery(string name, int pageIndex = 0, int pageNum = 20)
		{
			var dutiesQuery = _context.Duties.Where(d => d.Name != "NotSet");
			if (!name.IsNullOrEmpty()) dutiesQuery = dutiesQuery.Where(d => d.Name.Contains(name));
			var result = dutiesQuery.SplitPage(new DAL.QueryModel.QueryByPage() { PageIndex = pageIndex, PageSize = pageNum });
			var data = new EntitiesListDataModel<DutyDataModel>()
			{
				List = result.Item1.Select(d => d.ToDataModel()),
				TotalCount = result.Item2
			};
			return new JsonResult(new DutiesViewModel()
			{
				Data = data
			});
		}

		/// <summary>
		/// 检索可能的职务等级
		/// </summary>
		/// <param name="name"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageNum"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult TitleQuery(string name, int pageIndex = 0, int pageNum = 20)
		{
			var dutiesQuery = _context.UserCompanyTitles.Where(d => d.Name != "NotSet");
			if (!name.IsNullOrEmpty()) dutiesQuery = dutiesQuery.Where(d => d.Name.Contains(name));
			var result = dutiesQuery.SplitPage(new DAL.QueryModel.QueryByPage() { PageIndex = pageIndex, PageSize = pageNum });
			var data = new EntitiesListDataModel<UserTitleDataModel>()
			{
				List = result.Item1.Select(d => d.ToDataModel()),
				TotalCount = result.Item2
			};
			return new JsonResult(new UserTitlesViewModel()
			{
				Data = data
			});
		}

		/// <summary>
		/// 获取单位的子层级单位
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> CompanyChild(string id)
		{
			var currentUser = _currentUserService.CurrentUser;
			id = id ?? currentUser?.CompanyInfo?.Company?.Code;
			var list = _companiesService.FindAllChild(id)?.ToDictionary(c => c.Code) ?? new Dictionary<string, Company>();
			int totalCount = list.Count;
			var manageCount = 0;
			if (currentUser != null && (new List<string>() { null, "", "root" }.Contains(id)))
			{
				var mymanage_result = await _usersService.InMyManage(currentUser);
				manageCount = mymanage_result.Item2;
				foreach (var c in mymanage_result.Item1)
				{
					c.Name = $"*{c.Name}";
					if (!list.TryAdd(c.Code, c))
					{
						manageCount--;
						list[c.Code] = c;
					}
				}
			}
			return new JsonResult(new AllChildViewModel()
			{
				Data = new EntitiesListDataModel<CompanyChildDataModel>()
				{
					List = list.Values.OrderByDescending(c => c.Priority).Select(c => c.ToCompanyModel()),
					TotalCount = totalCount + manageCount
				}
			});
		}

		/// <summary>
		/// 获取单位详情
		/// </summary>
		/// <param name="id">单位code</param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		public IActionResult Detail(string id)
		{
			var c = _companiesService.GetById(id);
			return new JsonResult(new EntityViewModel<Company>(c));
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
			if (list == null) return new JsonResult(ActionStatusMessage.CompanyMessage.NotExist);
			return new JsonResult(new CompanyManagerViewModel()
			{
				Data = new CompanyManagerDataModel()
				{
					List = list.Select(u => u.ToSummaryDto())
				}
			});
		}

		/// <summary>
		/// 获取多个单位的管理
		/// </summary>
		/// <param name="ids"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(CompaniesManagerDataModel), 0)]
		public IActionResult CompaniesManagers(string ids)
		{
			var companiesCode = ids?.Split("##") ?? Array.Empty<string>();
			companiesCode.Distinct();
			var cmps = new Dictionary<string, CompanyManagerDataModel>();
			foreach (var c in companiesCode)
			{
				cmps.Add(c, new CompanyManagerDataModel()
				{
					List = _companiesService.GetCompanyManagers(c)?.Select(u => u.ToSummaryDto())
				});
			}
			var result = new CompaniesManagerDataModel() { Companies = cmps };
			return new JsonResult(new CompaniesManagerViewModel()
			{
				Data = result
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
			var list = _companyManagerServices.GetMembers(code, page, pageSize, out var totalCount).Select(u => u.ToSummaryDto());
			return new JsonResult(new AllMembersViewModel()
			{
				Data = new AllMembersDataModel()
				{
					List = list,
					TotalCount = totalCount
				}
			});
		}
	}
}