using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using Castle.Core.Internal;
using DAL.Data;
using DAL.DTO.Apply;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.Static;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

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
		private readonly IUsersService _usersService;
		private readonly ICompaniesService _companiesService;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="verifyService"></param>
		/// <param name="context"></param>
		/// <param name="vocationCheckServices"></param>
		/// <param name="hostingEnvironment"></param>
		/// <param name="applyService"></param>
		/// <param name="usersService"></param>
		/// <param name="companiesService"></param>
		public StaticController(IVerifyService verifyService, ApplicationDbContext context, IVocationCheckServices vocationCheckServices, IHostingEnvironment hostingEnvironment, IApplyService applyService, IUsersService usersService, ICompaniesService companiesService)
		{
			_verifyService = verifyService;
			_context = context;
			_vocationCheckServices = vocationCheckServices;
			_hostingEnvironment = hostingEnvironment;
			_applyService = applyService;
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
			if (!_verifyService.Status.IsNullOrEmpty())
			{
				var status = _verifyService.Status;
				_verifyService.Generate();
				return new JsonResult(new Status(ActionStatusMessage.Account.Auth.Verify.Invalid.status, status));
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
		[Route("verify-ft.png")]
		public IActionResult VerifyCodeFront()
		{
			var img = _verifyService.Front();
			if (img == null) return new JsonResult(new ApiDataModel()
			{
				Code = -1,
				Message = _verifyService.Status
			});
			HttpContext.Response.Cookies.Append("posY",_verifyService.Pos.Y.ToString());
			return new FileContentResult(img,"image/jpg");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[Route("verify-bg.png")]
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
			var location=_context.AdminDivisions.Find(code);
			if(location==null)return new JsonResult(ActionStatusMessage.Fail);
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
			if (location == null) return new JsonResult(ActionStatusMessage.Fail);
			var list = _context.AdminDivisions.Where(a => a.ParentCode == code);
			return new JsonResult(new LocationChildrenViewModel()
			{
				Data = new LocationChildrenDataModel()
				{
					List = list.Select(t=>t.ToDataModel())
				}
			});
		}
		/// <summary>
		/// 获取法定节假日情况
		/// </summary>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(VocationDescriptionDataModel), 0)]
		[Route("VocationDate")]
		public IActionResult VocationDate(DateTime start, int length)
		{
			var list = _vocationCheckServices.GetVocationDescriptions(start, length);
			return new JsonResult(new VocationDescriptionViewModel()
			{
				Data = new VocationDescriptionDataModel()
				{
					Descriptions	= list,
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
		[AllowAnonymous]
		[ProducesResponseType(typeof(string),0)]

		[Route("XlsExport")]
		public IActionResult XlsExport( XlsExportViewModel form)
		{
			var sWebRootFolder = _hostingEnvironment.WebRootPath;
			form.Templete = $"Templete\\{form.Templete}";
			var tempFile = new FileInfo(Path.Combine(sWebRootFolder, form.Templete));
			if(!tempFile.Exists)return new JsonResult(ActionStatusMessage.Static.TempXlsNotExist);

			byte[] fileContent = null;
			string fileName = DateTime.Now.ToString("yyyy年mm月dd日导出.xlsx");
			if (!form.Apply.IsNullOrEmpty())
			{
				Guid.TryParse(form.Apply, out var guid);
				if(guid==Guid.Empty)return new JsonResult(ActionStatusMessage.Apply.Operation.Invalid);
				var apply = _applyService.Get(guid)?.ToDetaiDto();
				if(apply==null)return new JsonResult(ActionStatusMessage.Apply.NotExist);
				fileContent=_applyService.ExportExcel(tempFile.FullName, apply);
				fileName = $"{apply.Base.RealName}的{apply.RequestInfo.VocationTotalLength()}天休假申请导出到{form.Templete}";
			}
			else
			{
				IEnumerable<DAL.Entities.ApplyInfo.Apply> list=null;
				string fromName = string.Empty;
				if (form.User != null)
				{
					list = _applyService.GetApplyBySubmitUser(form.User);
					var targetUser = _usersService.Get(form.User);
					fromName = targetUser?.BaseInfo.RealName ?? form.User;
				}
				else if (form.Company != null)
				{
					list = _applyService.GetApplyByToAuditCompany(form.Company);
					var targetCompany = _companiesService.Get(form.Company);
					fromName = targetCompany?.Name ?? form.Company;
				}
				else return new JsonResult(ActionStatusMessage.Apply.Operation.Invalid);
				if(list==null)return new JsonResult(ActionStatusMessage.Apply.NotExist);
				fileContent=_applyService.ExportExcel(tempFile.FullName, list.Select(a=>a.ToDetaiDto()));
				fileName = $"来自{fromName}的申请共计{list.Count()}条导出到{form.Templete}";
			}

			return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
		}
	}
}
