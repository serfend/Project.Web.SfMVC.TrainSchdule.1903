using BLL.Helpers;
using BLL.Interfaces.GameR3;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Game;

namespace TrainSchdule.Controllers.Game_r3
{
	/// <summary>
	/// 忍者必须死3的自动领礼包，需要前端配合
	/// </summary>
	[Route("[controller]/[action]")]
	public class R3Controller : Controller
	{
		private readonly IGameR3Services gameR3Services;
		private readonly IR3UsersServices r3UsersServices;

		public R3Controller(IGameR3Services gameR3Services, IR3UsersServices r3UsersServices)
		{
			this.gameR3Services = gameR3Services;
			this.r3UsersServices = r3UsersServices;
		}
		/// <summary>
		/// 领取当前礼品码
		/// </summary>
		/// <param name="userid"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		public async Task<IActionResult> HandleCode(string userid, string code)
		{
			var u = await gameR3Services.UpdateUser(new DAL.Entities.Game_r3.User() { GameId = userid });
			if (u.NickName == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var c = await gameR3Services.ShareCode(u.User, new DAL.Entities.Game_r3.GiftCode() { Code = code });
			var r = await gameR3Services.HandleCode(u.User, c);
			return new JsonResult(r.StatusDescription == null ? ActionStatusMessage.Success : new ApiResult() { Message = r.StatusDescription, Status = -1 });
		}

		/// <summary>
		/// 返回所有可用的礼品码
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> GiftCodes(string userid)
		{
			var list = await gameR3Services.GetAllValidGiftCodes().ConfigureAwait(true);
			var userhistory = await gameR3Services.GetUserGiftCodeHistory(new DAL.Entities.Game_r3.User() { GameId = userid }).ConfigureAwait(true);
			return new JsonResult(new UserGiftCodeGainHistoryViewModel()
			{
				Data = new UserGiftCodeGainHistoryDataModel()
				{
					List = list.Select(i => new GiftCodeDataModel()
					{
						Code = i,
						GainStamp = (userhistory.Where(h => h.Code.Code == i.Code).FirstOrDefault()?.GainStamp) ?? 0
					})
				}
			});
		}
		/// <summary>
		/// 分享新的礼品码
		/// </summary>
		/// <param name="userid"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		public async Task<IActionResult> ShareCode(string userid, string code)
		{
			var c = await gameR3Services.ShareCode(new DAL.Entities.Game_r3.User() { GameId = userid }, new DAL.Entities.Game_r3.GiftCode() { Code = code });
			return new JsonResult(new GiftCodeViewModel() { Data = new GiftCodeDataModel() { Code = c } });
		}
		/// <summary>
		/// 调取用户信息，同时注册此用户
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> UserInfo(string userid)
		{
			var u = await gameR3Services.UpdateUser(new DAL.Entities.Game_r3.User() { GameId = userid }).ConfigureAwait(true);
			return new JsonResult(new UserInfoViewModel()
			{
				Data = new UserInfoDataModel()
				{
					User = u
				}
			});
		}
		/// <summary>
		/// 获取领取记录
		/// </summary>
		/// <param name="userid">需要查询的人员</param>
		/// <param name="code">需要查询的礼品码</param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public async Task<IActionResult> GiftCodeHistory(string userid, string code, int pageIndex = 0, int pageSize = 20)
		{
			pageSize = pageSize > 20 ? 20 : pageSize;
			if (pageSize <= 0) return new JsonResult(ActionStatusMessage.User.Default);
			var list = await r3UsersServices.GainGiftCodeHistory(userid, code, pageIndex, pageSize).ConfigureAwait(true);
			return new JsonResult(new UserGiftCodeGainHistoryViewModel()
			{
				Data = new UserGiftCodeGainHistoryDataModel()
				{
					List = list.Select(i => new GiftCodeDataModel()
					{
						Code = i.Code,
						GainStamp = i.GainStamp,
						User = r3UsersServices.GetUser(i.User.GameId)?.NickName
					})
				}
			});
		}
		/// <summary>
		/// 获取登记在册的可爱用户
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> Members(int pageIndex = 0, int pageSize = 20)
		{
			pageSize = pageSize > 20 ? 20 : pageSize;
			if (pageSize <= 0) return new JsonResult(ActionStatusMessage.User.Default);
			var users = await r3UsersServices.Members(pageIndex, pageSize).ConfigureAwait(true);
			return new JsonResult(new UsersViewModel()
			{
				Data = new UsersDataModel()
				{
					Users = users
				}
			});
		}
	}
}
