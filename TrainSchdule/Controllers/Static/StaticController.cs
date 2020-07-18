using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BLL.Extensions;
using BLL.Extensions.ApplyExtensions;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.File;
using DAL.Data;
using DAL.DTO.Apply;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.Static;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;
using CollectionExtensions = Castle.Core.Internal.CollectionExtensions;

namespace TrainSchdule.Controllers
{
	/// <summary>
	///
	/// </summary>
	[Route("[controller]/[action]")]
	public partial class StaticController : Controller
	{
		private readonly IWebHostEnvironment env;
		private readonly IVerifyService _verifyService;
		private readonly IVacationCheckServices _vacationCheckServices;
		private readonly ApplicationDbContext _context;
		private readonly IApplyService _applyService;
		private readonly IHttpContextAccessor _httpContext;
		private readonly ICurrentUserService _currentUserService;
		private readonly IUsersService _usersService;
		private readonly ICompaniesService _companiesService;
		private readonly IFileServices _fileServices;

		/// <summary>
		///
		/// </summary>
		/// <param name="env"></param>
		/// <param name="verifyService"></param>
		/// <param name="vacationCheckServices"></param>
		/// <param name="context"></param>
		/// <param name="applyService"></param>
		/// <param name="currentUserService"></param>
		/// <param name="usersService"></param>
		/// <param name="companiesService"></param>
		/// <param name="httpContext"></param>
		/// <param name="fileServices"></param>
		public StaticController(IWebHostEnvironment env, IVerifyService verifyService, IVacationCheckServices vacationCheckServices, ApplicationDbContext context, IApplyService applyService, ICurrentUserService currentUserService, IUsersService usersService, ICompaniesService companiesService, IHttpContextAccessor httpContext, IFileServices fileServices)
		{
			this.env = env;
			_verifyService = verifyService;
			_vacationCheckServices = vacationCheckServices;
			_context = context;
			_applyService = applyService;
			_currentUserService = currentUserService;
			_usersService = usersService;
			_companiesService = companiesService;
			_httpContext = httpContext;
			_fileServices = fileServices;
		}

		/// <summary>
		/// 获取法定节假日情况
		/// </summary>
		/// <param name="start">开始日期</param>
		/// <param name="length">长度</param>
		/// <param name="caculateLawVacation">是否计算法定节假日</param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(VacationDescriptionDataModel), 0)]
		public async Task<IActionResult> VacationDate(DateTime start, int length, bool caculateLawVacation)
		{
			var list = await _vacationCheckServices.GetVacationDescriptions(start, length, caculateLawVacation).ConfigureAwait(true);
			return new JsonResult(new VacationDescriptionViewModel()
			{
				Data = new VacationDescriptionDataModel()
				{
					Descriptions = list,
					EndDate = _vacationCheckServices.EndDate,
					StartDate = start,
					VacationDays = _vacationCheckServices.EndDate.Subtract(start).Days
				}
			});
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(LocationDataModel), 0)]
		public IActionResult Location(int code)
		{
			var location = _context.AdminDivisions.Find(code);
			if (location == null) return new JsonResult(ActionStatusMessage.Static.AdminDivision.NoSuchArea);
			return new JsonResult(new LocationViewModel()
			{
				Data = new LocationDataModel()
				{
					Code = location.Code,
					ParentCode = location.ParentCode,
					Name = location.Name,
					ShortName = location.ShortName
				}
			});
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(LocationChildrenDataModel), 0)]
		public IActionResult LocationChildren(string code)
		{
			int.TryParse(code, out var codeInt);
			var location = _context.AdminDivisions.Find(codeInt);
			if (location == null) return new JsonResult(ActionStatusMessage.Static.AdminDivision.NoChildArea);
			var u = _currentUserService.CurrentUser;
			var list = _context.AdminDivisions.Where(a => a.ParentCode == codeInt).ToList();
			int divider = 1;
			while (divider < 1000000 && codeInt % 10 == 0)
			{
				divider *= 10;
				codeInt /= 10;
			}
			divider /= 100;
			var result = new List<AdminDivision>(list.Count);
			// 根据当前登录用户的家庭情况选择顺序
			if (u != null)
			{
				var firstResult = new List<AdminDivision>(4);
				var uSettle = u.SocialInfo.Settle;
				var uSelf = uSettle?.Self?.Address?.Code ?? -1;
				var uParent = uSettle?.Parent?.Address?.Code ?? -1;
				var uLover = uSettle?.Lover?.Address?.Code ?? -1;
				var uLoverParent = uSettle?.LoversParent?.Address?.Code ?? -1;
				var targets = new List<int>() { uSelf, uParent, uLover, uLoverParent };
				foreach (var l in list)
				{
					if (targets.Any(c => Math.Abs(c - l.Code) < divider))
					{
						firstResult.Add(l);
						HttpContext.Response.Headers["X-Priority"] += $" {l.Code}";
					}
					else result.Add(l);
				}
				if (firstResult.Count > 0)
				{
					firstResult.AddRange(result);
					result = firstResult;
				}
			}
			else
				result = list;
			var totalCount = result.Count;
			return new JsonResult(new LocationChildrenViewModel()
			{
				Data = new LocationChildrenDataModel()
				{
					List = result.Select(t => t.ToDataModel()),
					TotalCount = totalCount
				}
			});
		}

		/// <summary>
		/// 上传休假导出模板
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(typeof(string), 0)]
		public IActionResult XlsExport(IEnumerable<IFormFile> file)
		{
			return new JsonResult(new { file });
		}
	}
}