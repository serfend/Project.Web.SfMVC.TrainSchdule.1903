using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Extensions;
using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.Permission;
using Castle.Core.Internal;
using DAL.Data;
using DAL.DTO.User;
using DAL.Entities;
using DAL.Entities.UserInfo;
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
	public partial class CompanyController : Controller
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
		/// 单位类别查询
		/// </summary>
		/// <param name="tag"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult CompanyTag(string tag, int pageIndex = 0, int pageSize = 20)
		{
			var pattern = $"%{tag}%";
			var companyQuery = _context.CompaniesDb.AsQueryable();
			if (!tag.IsNullOrEmpty()) companyQuery = companyQuery.Where(d => EF.Functions.Like(d.Tag, pattern));
			var result = companyQuery.AsEnumerable()
				.SelectMany(d => d.Tag?.Split("##", StringSplitOptions.RemoveEmptyEntries) ?? new List<string>().ToArray())
				.Where(d => EF.Functions.Like(d, pattern))
				.Distinct()
				.OrderBy(a => a)
				.SplitPage(new DAL.QueryModel.QueryByPage() { PageIndex = pageIndex, PageSize = pageSize });
			return new JsonResult(new EntitiesListViewModel<string>(result.Item1, result.Item2));
		}

		private int AddCompanyList(Dictionary<string, Company> list, List<Company> raw_list)
		{
			int result = 0;
			foreach (var c in raw_list)
			{
				c.Name = $"*{c.Name}";
				if (!list.TryAdd(c.Code, c))
				{
					result--;
					list[c.Code] = c;
				}
			}
			return result;
		}

		/// <summary>
		/// 获取单位的子层级单位
		/// 默认返回用户自身单位及用户可管辖单位
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> CompanyChild(string id)
		{
			var currentUser = _currentUserService.CurrentUser;
			id ??= currentUser?.CompanyInfo?.Company?.Code;
			var list = _companiesService.FindAllChild(id)?.ToDictionary(c => c.Code) ?? new Dictionary<string, Company>();
			int totalCount = list.Count;
			var manageCount = 0;
			if (currentUser != null && (new List<string>() { null, "", "root" }.Contains(id)))
			{
				var mymanage_result = await _usersService.InMyManage(currentUser);
				manageCount = mymanage_result.Item2;
				var uc = currentUser.CompanyInfo.Company;
				list[uc.Code] = uc;
				manageCount -= AddCompanyList(list, mymanage_result.Item1.ToList());
				var permissionCompanies = _companiesService.PermissionViewCompanies(currentUser);
				manageCount += permissionCompanies.Count;
				manageCount -= AddCompanyList(list, permissionCompanies);
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
			if (c == null) id = _currentUserService.CurrentUser?.CompanyInfo?.Company?.Code;
			c = _companiesService.GetById(id);

			return new JsonResult(new EntityViewModel<Company>(c));
		}

		/// <summary>
		/// 获取单位的主管
		/// </summary>
		/// <param name="id">单位代码</param>
		/// <param name="userid">用户代码</param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(CompanyManagerDataModel), 0)]
		public IActionResult Managers(string id, string userid)
		{
			id = id ?? _currentUserService.CurrentUser?.CompanyInfo?.Company?.Code;
			var list = _companiesService.GetCompanyManagers(id, userid);
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
			if (companiesCode.Length > 20) return new JsonResult(ActionStatusMessage.Success);
			var cmps = new Dictionary<string, CompanyManagerDataModel>();
			foreach (var c in companiesCode)
			{
				cmps.Add(c, new CompanyManagerDataModel()
				{
					List = _companiesService.GetCompanyManagers(c, null)?.Select(u => u.ToSummaryDto())
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
			int totalCount = 0;
			var list = code == null ? new List<UserSummaryDto>() : _companyManagerServices.GetMembers(code, page, pageSize, out totalCount).Select(u => u.ToSummaryDto());
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