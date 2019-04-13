using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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
			return new JsonResult(_verifyService.Generate());
			//return new FileStreamResult(_verifyService.Generate(), "image/png");
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
