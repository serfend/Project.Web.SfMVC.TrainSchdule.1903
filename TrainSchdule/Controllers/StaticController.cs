using System;
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
	/// <summary>
	/// 
	/// </summary>
	[Route("[controller]")]
	public class StaticController : Controller
	{

		private readonly IVerifyService _verifyService;
		private readonly IVocationCheckServices _vocationCheckServices;
		private readonly ApplicationDbContext _context;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="verifyService"></param>
		/// <param name="context"></param>
		public StaticController(IVerifyService verifyService, ApplicationDbContext context, IVocationCheckServices vocationCheckServices)
		{
			_verifyService = verifyService;
			_context = context;
			_vocationCheckServices = vocationCheckServices;
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
	}
}
