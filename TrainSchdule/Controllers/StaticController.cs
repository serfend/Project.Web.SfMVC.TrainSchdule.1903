using BLL.Helpers;
using BLL.Interfaces;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TrainSchdule.ViewModels.Verify;
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
	}
}
