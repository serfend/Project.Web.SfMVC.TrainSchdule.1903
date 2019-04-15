using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TrainSchdule.BLL.Helpers;
using TrainSchdule.ViewModels.Static;
using TrainSchdule.Web.ViewModels;

namespace TrainSchdule.Controllers
{
	[Route("[controller]")]
	public class StaticController : Controller
	{

		private readonly IVerifyService _verifyService;

		public StaticController(IVerifyService verifyService)
		{
			_verifyService = verifyService;
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
				return new JsonResult(new Status(ActionStatusMessage.AccountLogin_InvalidByUnknown.Code, status));
			}
			return new JsonResult(new VerifyGeneratedViewModel()
			{
				Id= imgId,
				PosY = _verifyService.Pos.Y
			});
		}
		[HttpGet]
		[AllowAnonymous]
		[Route("verify-ft.png")]
		public IActionResult VerifyCodeFront()
		{
			var img = _verifyService.Front();
			if (img == null) return new JsonResult(new APIViewModel()
			{
				Code = -1,
				Message = _verifyService.Status
			});
			return new FileContentResult(img,"image/png");
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("verify-bg.png")]
		public IActionResult VerifyCodeBackground()
		{
			var img = _verifyService.Background();
			if (img == null) return new JsonResult(new APIViewModel()
			{
				Code = -1,
				Message = _verifyService.Status
			});
			return new FileContentResult(img, "image/png");
		}
	}
}
