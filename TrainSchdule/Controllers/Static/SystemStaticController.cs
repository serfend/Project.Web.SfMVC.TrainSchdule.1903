using BLL.Helpers;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
	public partial class StaticController
	{
		/// <summary>
		/// 获取图像的base64
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		public IActionResult ImgToBase64(IFormFile file)
		{
			if (file == null) return new JsonResult(ActionStatusMessage.Static.FileNotExist);
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
		public IActionResult LocationChildren(int code)
		{
			var location = _context.AdminDivisions.Find(code);
			if (location == null) return new JsonResult(ActionStatusMessage.Static.AdminDivision.NoChildArea);
			var u = _currentUserService.CurrentUser;
			var list = _context.AdminDivisions.Where(a => a.ParentCode == code).ToList();
			int divider = 1;
			while (divider < 1000000 && code % 10 == 0)
			{
				divider *= 10;
				code /= 10;
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
	}
}