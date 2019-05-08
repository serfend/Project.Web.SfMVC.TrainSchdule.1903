using System.Linq;
using BLL.Helpers;
using BLL.Interfaces;
using Castle.Core.Internal;
using DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.Static;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers
{
	[Route("[controller]")]
	public class StaticController : Controller
	{

		private readonly IVerifyService _verifyService;
		private readonly ApplicationDbContext _context;

		public StaticController(IVerifyService verifyService, ApplicationDbContext context)
		{
			_verifyService = verifyService;
			_context = context;
		}

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
		[HttpGet]
		[AllowAnonymous]
		[Route("verify-ft.png")]
		public IActionResult VerifyCodeFront()
		{
			var img = _verifyService.Front();
			if (img == null) return new JsonResult(new APIDataModel()
			{
				Code = -1,
				Message = _verifyService.Status
			});
			HttpContext.Response.Cookies.Append("posY",_verifyService.Pos.Y.ToString());
			return new FileContentResult(img,"image/jpg");
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("verify-bg.png")]
		public IActionResult VerifyCodeBackground()
		{
			var img = _verifyService.Background();
			if (img == null) return new JsonResult(new APIDataModel()
			{
				Code = -1,
				Message = _verifyService.Status
			});
			HttpContext.Response.Cookies.Append("posY", _verifyService.Pos.Y.ToString());
			return new FileContentResult(img, "image/jpg");
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("Location")]
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
		[HttpGet]
		[AllowAnonymous]
		[Route("LocationChildren")]
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
	}
}
