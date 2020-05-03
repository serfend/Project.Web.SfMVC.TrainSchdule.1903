using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.Common;
using DAL.Entities;
using DAL.QueryModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.Extensions;
using TrainSchdule.Extensions.Common;
using TrainSchdule.ViewModels.Common;
using TrainSchdule.ViewModels.Static;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers
{
	/// <summary>
	/// 短网址管理
	/// </summary>
	[Authorize]
	public class DWZController : Controller
	{
		private readonly IDWZServices dWZServices;
		private readonly ICurrentUserService currentUserService;
		private readonly IUserActionServices userActionServices;

		/// <summary>
		/// 短网址
		/// </summary>
		/// <param name="dWZServices"></param>
		/// <param name="currentUserService"></param>
		/// <param name="userActionServices"></param>
		public DWZController(IDWZServices dWZServices, ICurrentUserService currentUserService, IUserActionServices userActionServices)
		{
			this.dWZServices = dWZServices;
			this.currentUserService = currentUserService;
			this.userActionServices = userActionServices;
		}

		/// <summary>
		/// 短网址
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		[Route("s/{url}")]
		[AllowAnonymous]
		[HttpGet]
		public IActionResult RedirectDwz([FromRoute]string url)
		{
			var m = dWZServices.Load(url);
			var c = currentUserService.CurrentUser;
			if (m == null) return new JsonResult(ActionStatusMessage.Static.ResourceNotExist);
			return Redirect(dWZServices.Open(m, c));
		}

		/// <summary>
		/// 删除一个短网址
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		[Route("s/{url}")]
		[HttpDelete]
		public IActionResult RemoveDwz([FromRoute] string url)
		{
			var c = currentUserService.CurrentUser;
			var m = dWZServices.Load(url);
			if (m == null) return new JsonResult(ActionStatusMessage.Static.ResourceNotExist);
			var permit = userActionServices.Permission(c.Application.Permission, DictionaryAllPermission.Resources.ShortUrl, Operation.Remove, c.Id, m.CreateBy.CompanyInfo.Company.Code);
			if (!permit) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			dWZServices.Remove(m);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 查询短链
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[Route("Static/ShortUrl/Query")]
		[HttpPost]
		public IActionResult QueryDwz([FromBody]QueryDwzViewModel model)
		{
			var list = dWZServices.Query(model, out var totalCount);
			return new JsonResult(new ShortUrlsViewModel()
			{
				Data = new ShortUrlsDataModel()
				{
					List = list?.Select(s => s.ToDataModel()) ?? new List<ShortUrlCreateDataModel>(),
					TotalCount = totalCount
				}
			});
		}

		/// <summary>
		/// 创建短网址
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("Static/ShortUrl/Create")]
		public IActionResult Create([FromBody]ShortUrlCreateDataModel model)
		{
			var c = currentUserService.CurrentUser;
			var permit = userActionServices.Permission(c.Application.Permission, DictionaryAllPermission.Resources.ShortUrl, Operation.Create, c.Id, null);
			if (!permit) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			try
			{
				var m = dWZServices.Create(c, model.Target, model.Expire, model.Key);
				return new JsonResult(new ShortUrlViewModel()
				{
					Data = m.ToDataModel()
				});
			}
			catch (ActionStatusMessageException ex)
			{
				return new JsonResult(ex.Status);
			}
		}

		/// <summary>
		/// 查询短网址访问统计
		/// </summary>
		/// <param name="key"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		[Route("s/{key}/Statistics")]
		[HttpPost]
		public IActionResult QueryStatistics([FromRoute] string key, [FromBody]QueryDwzStatisticsViewModel model)
		{
			var c = currentUserService.CurrentUser;
			var m = dWZServices.Load(key);
			if (m == null) return new JsonResult(ActionStatusMessage.Static.ResourceNotExist);
			if (!userActionServices.Permission(c.Application.Permission, DictionaryAllPermission.Resources.ShortUrl, Operation.Query, c.Id, m.CreateBy.CompanyInfo.Company.Code)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var statistics = dWZServices.QueryStatistics(m, model, out var totalCount);
			return new JsonResult(new ShortUrlStatisticsViewModel()
			{
				Data = new ShortUrlStatisticsDataModel()
				{
					ShortUrl = m.ToDataModel(),
					Statistics = statistics.Select(s => s.ToDataModel()),
					TotalCount = totalCount
				}
			});
		}
	}
}