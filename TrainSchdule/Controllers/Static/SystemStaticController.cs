using BLL.Helpers;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.Static;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers
{
	/// <summary>
	/// 系统内部的静态
	/// </summary>
	public class SystemStaticController : Controller
	{
		private readonly IConfiguration configuration;
		private readonly IVerifyService _verifyService;

		/// <summary>
		///
		/// </summary>
		/// <param name="configuration"></param>
		/// <param name="verifyService"></param>
		public SystemStaticController(IConfiguration configuration, IVerifyService verifyService)
		{
			this.configuration = configuration;
			this._verifyService = verifyService;
		}

		/// <summary>
		/// 获取图像的base64
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		public IActionResult ImgToBase64(IFormFile file)
		{
			if (file == null) return new JsonResult(ActionStatusMessage.StaticMessage.FileNotExist);
			using (var img = file.OpenReadStream())
			{
				var imgBuffer = new byte[file.Length];
				img.Read(imgBuffer, 0, (int)file.Length);
				return new JsonResult(new ResponseDataTViewModel<string>()
				{
					Data = $"{ Convert.ToBase64String(imgBuffer)}"
				});
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		public IActionResult VerifyCode()
		{
			var imgId = _verifyService.Generate().ToString();
			if (_verifyService.Status != null)
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
		public IActionResult VerifyCodeFront()
		{
			var img = _verifyService.Front();
			if (img == null) return new JsonResult(new ApiResult()
			{
				Status = -1,
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
		public IActionResult VerifyCodeBackground()
		{
			var img = _verifyService.Background();
			if (img == null) return new JsonResult(new ApiResult()
			{
				Status = -1,
				Message = _verifyService.Status
			});
			HttpContext.Response.Cookies.Append("posY", _verifyService.Pos.Y.ToString());
			return new FileContentResult(img, "image/jpg");
		}

		/// <summary>
		/// 获取服务器时间
		/// </summary>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet]
		public IActionResult TimeZone()
		{
			var config = configuration.GetSection("Configuration");
			var timeZone = config?.GetSection("TimeZone");
			var left = timeZone?.GetSection("Left");
			var leftName = left?["Name"] ?? "天文时间";
			var leftValue = Convert.ToInt64(left?["Value"] ?? "0");
			var right = timeZone?.GetSection("Right");
			var rightName = right?["Name"] ?? "中心时间";
			var rightValue = Convert.ToInt64(right?["Value"] ?? "432000000");
			return new JsonResult(new TimeZoneViewModel()
			{
				Data = new TimeZoneDataModel()
				{
					Left = new ValueNameDataModel<DateTime>()
					{
						Name = leftName,
						Value = DateTime.Now.AddMilliseconds(leftValue)
					},
					Right = new ValueNameDataModel<DateTime>()
					{
						Name = rightName,
						Value = DateTime.Now.AddMilliseconds(rightValue)
					}
				}
			});
		}

		/// <summary>
		/// 返回查询状态
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public IActionResult QueryStatus([FromBody]string content)
		{
			var request = HttpContext.Request;
			return new JsonResult(new
			{
				Header = request.Headers,
				Query = request.Query,
				Cookies = request.Cookies,
				Body = content,
				Path = request.PathBase
			});
		}
	}
}