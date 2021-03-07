using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.Common;
using BLL.Services.Common;
using DAL.Entities;
using DAL.Entities.Common;
using DAL.QueryModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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
		/// <param name="dwzInnerServices"></param>
		/// <returns></returns>
		[Route("s/{url}")]
		[AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> RedirectDwz([FromRoute] string url, [FromServices] IDWZServices dwzInnerServices)
		{
			var m = await dWZServices.Load(url).ConfigureAwait(true);
			if (m == null) return new JsonResult(ActionStatusMessage.StaticMessage.ResourceNotExist);
			// Record(m); // 此处异步不会等待，所以服务器直接返回的同时把db清理了，导致Record方法报错
			await dwzInnerServices.Open(m, currentUserService.CurrentUser);
			return Redirect(m.Target);
		}

		private async Task Record(ShortUrl m)
		{
			var c = currentUserService.CurrentUser;
			await dWZServices.Open(m, c).ConfigureAwait(false);
		}

		/// <summary>
		/// 删除一个短网址
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		[Route("s/{url}")]
		[HttpDelete]
		public async Task<IActionResult> RemoveDwz([FromRoute] string url)
		{
			var c = currentUserService.CurrentUser;
			var m = await dWZServices.Load(url).ConfigureAwait(true);
			if (m == null) return new JsonResult(ActionStatusMessage.StaticMessage.ResourceNotExist);
			var permit = userActionServices.Permission(c.Application.Permission, DictionaryAllPermission.Resources.ShortUrl, Operation.Remove, c.Id, m.CreateBy.CompanyInfo.CompanyCode);
			if (!permit) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			await dWZServices.Remove(m).ConfigureAwait(false);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 查询短链
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[Route("Static/ShortUrl/Query")]
		[HttpPost]
		public async Task<IActionResult> QueryDwz([FromBody] QueryDwzViewModel model)
		{
			var result = await dWZServices.Query(model).ConfigureAwait(true);
			var list = result.Item1;
			var totalCount = result.Item2;
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
		public async Task<IActionResult> Create([FromBody] ShortUrlCreateDataModel model)
		{
			var c = currentUserService.CurrentUser;
			var permit = userActionServices.Permission(c.Application.Permission, DictionaryAllPermission.Resources.ShortUrl, Operation.Create, c.Id, null);
			if (!permit) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			try
			{
				var m = await dWZServices.Create(c, model.Target, model.Expire, model.Key).ConfigureAwait(true);
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
		public async Task<IActionResult> QueryStatistics([FromRoute] string key, [FromBody] QueryDwzStatisticsViewModel model)
		{
			var c = currentUserService.CurrentUser;
			var m = await dWZServices.Load(key).ConfigureAwait(true);
			if (m == null) return new JsonResult(ActionStatusMessage.StaticMessage.ResourceNotExist);
			if (!userActionServices.Permission(c.Application.Permission, DictionaryAllPermission.Resources.ShortUrl, Operation.Query, c.Id, m.CreateBy.CompanyInfo.CompanyCode)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var result = await dWZServices.QueryStatistics(m, model).ConfigureAwait(true);
			var statistics = result.Item1;
			var totalCount = result.Item2;
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