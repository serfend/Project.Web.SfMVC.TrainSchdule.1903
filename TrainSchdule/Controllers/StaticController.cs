using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BLL.Extensions;
using BLL.Extensions.ApplyExtensions;
using BLL.Helpers;
using BLL.Interfaces;
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
	[Route("[controller]")]
	public class StaticController : Controller
	{

		private readonly IVerifyService _verifyService;
		private readonly IVocationCheckServices _vocationCheckServices;
		private readonly ApplicationDbContext _context;
		private readonly IApplyService _applyService;
		private readonly IHostingEnvironment _hostingEnvironment;
		private readonly ICurrentUserService _currentUserService;
		private readonly IUsersService _usersService;
		private readonly ICompaniesService _companiesService;

		public StaticController(IVerifyService verifyService, IVocationCheckServices vocationCheckServices, ApplicationDbContext context, IApplyService applyService, IHostingEnvironment hostingEnvironment, ICurrentUserService currentUserService, IUsersService usersService, ICompaniesService companiesService)
		{
			_verifyService = verifyService;
			_vocationCheckServices = vocationCheckServices;
			_context = context;
			_applyService = applyService;
			_hostingEnvironment = hostingEnvironment;
			_currentUserService = currentUserService;
			_usersService = usersService;
			_companiesService = companiesService;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[Route("verify")]
		public IActionResult VerifyCode()
		{
			var imgId = _verifyService.Generate().ToString();
			if (!CollectionExtensions.IsNullOrEmpty(_verifyService.Status))
			{
				var status = _verifyService.Status;
				_verifyService.Generate();
				return new JsonResult(new ApiResult(ActionStatusMessage.Account.Auth.Verify.Invalid.Status, status));
			}

			return new JsonResult(new ScrollerVerifyGeneratedViewModel()
			{
				Data = new ScrollerVerifyGeneratedDataModel()
				{
					Id = imgId,
					PosY = _verifyService.Pos.Y
				}
			});
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[Route("verify-ft.jpg")]
		public IActionResult VerifyCodeFront()
		{
			var img = _verifyService.Front();
			if (img == null) return new JsonResult(new ApiDataModel()
			{
				Code = -1,
				Message = _verifyService.Status
			});
			HttpContext.Response.Cookies.Append("posY", _verifyService.Pos.Y.ToString());
			return new FileContentResult(img, "image/jpg");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[Route("verify-bg.jpg")]
		public IActionResult VerifyCodeBackground()
		{
			var img = _verifyService.Background();
			if (img == null) return new JsonResult(new ApiDataModel()
			{
				Code = -1,
				Message = _verifyService.Status
			});
			HttpContext.Response.Cookies.Append("posY", _verifyService.Pos.Y.ToString());
			return new FileContentResult(img, "image/jpg");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[Route("Location")]
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
		[Route("LocationChildren")]
		[ProducesResponseType(typeof(LocationChildrenDataModel), 0)]

		public IActionResult LocationChildren(int code)
		{
			var location = _context.AdminDivisions.Find(code);
			if (location == null) return new JsonResult(ActionStatusMessage.Static.AdminDivision.NoChildArea);
			var list = _context.AdminDivisions.Where(a => a.ParentCode == code);
			var totalCount = list.Count();
			return new JsonResult(new LocationChildrenViewModel()
			{
				Data = new LocationChildrenDataModel()
				{
					List = list.Select(t => t.ToDataModel()),
					TotalCount=totalCount
				}
			});
		}
		/// <summary>
		/// 获取法定节假日情况
		/// </summary>
		/// <param name="start">开始日期</param>
		/// <param name="length">长度</param>
		/// <param name="caculateLawVocation">是否计算法定节假日</param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(VocationDescriptionDataModel), 0)]
		[Route("VocationDate")]
		public IActionResult VocationDate(DateTime start, int length,bool caculateLawVocation)
		{
			var list = _vocationCheckServices.GetVocationDescriptions(start, length, caculateLawVocation);
			return new JsonResult(new VocationDescriptionViewModel()
			{
				Data = new VocationDescriptionDataModel()
				{
					Descriptions = list,
					EndDate = _vocationCheckServices.EndDate,
					StartDate = start,
					VocationDays = _vocationCheckServices.EndDate.Subtract(start).Days
				}
			});
		}
		/// <summary>
		/// 依据模板导出Xls
		/// </summary>
		/// <param name="form"></param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(string), 0)]
		[Route("XlsExport")]
		public IActionResult XlsExport(XlsExportViewModel form)
		{
			//var currentUser = _currentUserService.CurrentUser;
			var sWebRootFolder = _hostingEnvironment.WebRootPath;
			form.Templete = $"Templete\\{form.Templete}";
			var tempFile = new FileInfo(Path.Combine(sWebRootFolder, form.Templete));
			if (!tempFile.Exists) return new JsonResult(ActionStatusMessage.Static.TempXlsNotExist);

			byte[] fileContent = null;
			string fileName = DateTime.Now.ToString("yyyy年mm月dd日导出.xlsx");
			if (form.StatisticsId != null && form.Company != null)
			{
				//TODO 后续导出报告使用
			}
			else if (!CollectionExtensions.IsNullOrEmpty(form.Apply))
			{
				Guid.TryParse(form.Apply, out var guid);
				if (guid == Guid.Empty) return new JsonResult(ActionStatusMessage.Apply.GuidFail);
				var a = _applyService.GetById(guid);
				var apply = a?.ToDetaiDto(_usersService.VocationInfo(a.BaseInfo.From), false);
				if (apply == null) return new JsonResult(ActionStatusMessage.Apply.NotExist);
				//TODO 需要校验权限
				//if (!currentUser.Application.Permission.Check(DictionaryAllPermission.Apply.Default, Operation.Update, apply.Company)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
				fileContent = _applyService.ExportExcel(tempFile.FullName, apply);
				fileName = $"{apply.Base.RealName}的{apply.RequestInfo.VocationTotalLength()}天休假申请导出到{form.Templete}";
			}
			else
			{
				IEnumerable<DAL.Entities.ApplyInfo.Apply> list = null;
				string fromName = string.Empty;
				Company targetCompany = null;
				if (form.User != null)
				{
					list = _applyService.QueryApplies(new DAL.QueryModel.QueryApplyDataModel()
					{
						CreateFor = new DAL.QueryModel.QueryByString()
						{
							Value = form.User
						}
					},true,out var totalCount);
					var targetUser = _usersService.Get(form.User);
					fromName = targetUser?.BaseInfo.RealName ?? form.User;
					targetCompany = targetUser?.CompanyInfo?.Company;
				}
				else if (form.Company != null)
				{
					list = _applyService.QueryApplies(new DAL.QueryModel.QueryApplyDataModel()
					{
						CreateCompany = new DAL.QueryModel.QueryByString()
						{
							Value = form.Company
						}
					},true,out var totalCount);
					targetCompany = _companiesService.GetById(form.Company);
					fromName = targetCompany?.Name ?? form.Company;
				}
				else return new JsonResult(ActionStatusMessage.Apply.Operation.Invalid);
				if (list == null) return new JsonResult(ActionStatusMessage.Apply.NotExist);
				list = list.Where(a =>
					a.Status != AuditStatus.NotPublish && a.Status != AuditStatus.NotSave &&
					a.Status != AuditStatus.Withdrew).ToList();
				fileContent = _applyService.ExportExcel(tempFile.FullName, list.Select(a => a.ToDetaiDto(_usersService.VocationInfo(a.BaseInfo.From), false)), targetCompany?.ToDto(_companiesService));
				if (fileContent == null) return new JsonResult(ActionStatusMessage.Static.XlsNoData);
				fileName = $"来自{fromName}的申请共计{list.Count()}条导出到{form.Templete}";
			}

			return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
		}
		[AllowAnonymous]
		[HttpPost]
		[ProducesResponseType(typeof(string), 0)]
		[Route("file")]
		public IActionResult File(IEnumerable<IFormFile> file)
		{
			return new JsonResult(new { file });
		}
	}
}
