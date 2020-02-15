using BLL.Interfaces.GameR3;
using BLL.Services.Game_r3.R3ResultContent;
using DAL.Data;
using DAL.Entities.Game_r3;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.GameR3
{
	public class R3HandleServices : IGameR3Services
	{
		private readonly ApplicationDbContext context;
		private readonly HttpClient http;
		private const string host = "http://statistics.pandadastudio.com";

		public R3HandleServices(ApplicationDbContext context)
		{
			this.http = new HttpClient();
			this.context = context;
		}

		public async Task<GiftCode> HandleCode(User user, GiftCode code)
		{
			if (code == null) return code;
			if (user == null) return null;
			var gainHistory = context.GainGiftCodeHistory.Where(h => h.User.Id == user.Id&&h.Code.Code==code.Code).FirstOrDefault();
			if (gainHistory != null)
			{
				// 当领取过时，不再领取
				code.StatusDescription = $"{new DateTime(1970,1,1).AddMilliseconds(gainHistory.GainStamp)} 已领取过";
				return code;
			}
			var r = await http.GetAsync(new Uri($"{host}/player/giftCode?uid={user.GameId}&code={code.Code}")).ConfigureAwait(true);
			var content = await r.Content.ReadAsStringAsync().ConfigureAwait(true);
			var c = JsonConvert.DeserializeObject<GiftCodeResult>(content);
			switch (c.Code)
			{
				case "0":
				case "425":
					code.StatusDescription = c.Code == "425" ? "已领取过" : code.StatusDescription;
					code.Valid = true;
					// 领取成功则记录本次领取
					gainHistory = new GainGiftCode()
					{
						Code = code,
						GainStamp = Convert.ToInt64(DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),
						User = user
					};
					context.GainGiftCodeHistory.Add(gainHistory);
					break;
				default:
					{
						code.InvalidTime = DateTime.Now;
						code.StatusDescription = $"[{c.Code}]{c.Message}";
						context.GiftCodes.Update(code);
						break;
					}
			}
			await context.SaveChangesAsync().ConfigureAwait(true);
			return code;
		}

		public async Task<GiftCode> ShareCode(User user, GiftCode code)
		{
			if (code == null) return null;
			var u =await UpdateUser(user).ConfigureAwait(true);
			if (u.NickName == null) return null;
			var prevCode = context.GiftCodes.Where(g => g.Code == code.Code).FirstOrDefault();
			if (prevCode != null) return prevCode;
			code.ShareBy = u.User;
			code.ShareTime = DateTime.Now;
			code = await HandleCode(code.ShareBy, code).ConfigureAwait(true);
			context.GiftCodes.Update(code);
			await context.SaveChangesAsync().ConfigureAwait(true);
			return code;
		}

		public async Task<User> UpdateHandleInterval(User user, long interval)
		{
			if (user == null || interval < 3600 * 1000) return user;
			user.HandleInterval = interval;
			context.GameR3Users.Update(user);
			await context.SaveChangesAsync().ConfigureAwait(true);
			return user;
		}

		public async Task<UserInfo> UpdateUser(User user)
		{
			if (user == null) return null;
			var prevUser = context.GameR3Users.Where(u => u.GameId == user.GameId).FirstOrDefault();
			if (prevUser == null)
			{
				// 首次注册时新增用户
				user.Id = Guid.Empty;
				user.HandleInterval = 1000 * 3600;
				context.GameR3Users.Add(user);
			}
			var r = await http.GetAsync(new Uri($"{host}/player/simpleInfo?uid={user.GameId}")).ConfigureAwait(true);
			var content = await r.Content.ReadAsStringAsync().ConfigureAwait(true);
			var c = JsonConvert.DeserializeObject<SimpleUserInfoResult>(content);
			var prevUserInfo = context.GameR3UserInfos.Where(u => u.User.GameId == user.GameId).FirstOrDefault();
			// 用户不存在时不记录
			user.Enable = c.Code == "0";
			if (prevUserInfo == null) prevUserInfo = new UserInfo()
			{
				User = user
			};
			if (prevUserInfo.NickName != c.Data?.NickName || prevUserInfo.Level != c.Data?.Level)
			{
				var u = new UserInfo()
				{
					User = user,
					NickName = c.Data.NickName,
					Level = c.Data.Level,
					DateTime = DateTime.Now
				};
				context.GameR3UserInfos.Add(u);
				prevUserInfo = u;
			}
			await context.SaveChangesAsync().ConfigureAwait(true);
			return prevUserInfo;
		}

		public async Task<IEnumerable<GiftCode>> GetAllValidGiftCodes()
		{
			var codes = context.GiftCodes.Where(c => c.Valid);
			return codes.ToList();
		}

		public async Task HandleAllUsersGiftCode()
		{
			var nowStamp = (long)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
			var list = await GetAllValidGiftCodes().ConfigureAwait(true);
			var handleUsers = context.GameR3Users.Where(u=>u.Enable).Where(u => u.LastHandleStamp + u.HandleInterval > nowStamp);
			foreach (var u in handleUsers)
			{
				u.LastHandleStamp = nowStamp;
				var uAfter =await UpdateUser(u).ConfigureAwait(true);
				if (uAfter == null)
				{
					continue;
				}
				foreach (var c in list)
				{
					var cAfter = await HandleCode(u, c).ConfigureAwait(true);
					await Task.Delay(cAfter.Valid ? 500 : 1500).ConfigureAwait(true);
					if (!cAfter.Valid) break;
				}
			}
		}

		public async Task<IEnumerable<GainGiftCode>> GetUserGiftCodeHistory(User user)
		{
			var list = context.GainGiftCodeHistory.Where(h => h.User.GameId == user.GameId);
			return list.ToList();
		}
	}
}
