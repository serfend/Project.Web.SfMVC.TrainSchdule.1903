using BLL.Helpers;
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
	public class R3HandleServices : IGameR3Services, IDisposable
	{
		private readonly ApplicationDbContext context;
		private readonly HttpClient http;
		private bool disposedValue;
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
			var gainHistory = context.GainGiftCodeHistory.Where(h => h.User.Id == user.Id && h.Code.Code == code.Code).FirstOrDefault();
			if (gainHistory != null)
			{
				// 当领取过时，不再领取
				code.StatusDescription = $"{DateTime.FromFileTime(new DateTime(1970, 1, 1).ToFileTime() + (gainHistory.GainStamp))} 已领取过";
				context.GiftCodes.Update(code);
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
						GainStamp = Convert.ToInt64(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),
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
			var u = await UpdateUser(user).ConfigureAwait(true);
			if (u.NickName == null) throw new ActionStatusMessageException(ActionStatusMessage.UserMessage.NotExist, $"忍忍{user.GameId}不存在");
			var prevCode = context.GiftCodes.Where(g => g.Code == code.Code).FirstOrDefault();
			if (prevCode != null) return prevCode;
			code.ShareBy = $"{u.Level} {u.NickName}";
			code.ShareTime = DateTime.Now;
			code = await HandleCode(u.User, code).ConfigureAwait(true);
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
			}
			context.GameR3UserInfos.Update(prevUserInfo);
			await context.SaveChangesAsync().ConfigureAwait(true);
			return prevUserInfo;
		}

		public async Task<IEnumerable<GiftCode>> GetAllValidGiftCodes(int pageIndex, int pageSize)
		{
			return await Task.Run(() =>
			{
				var codes = context.GiftCodes.Where(c => c.Valid);
				codes = codes.OrderByDescending(c => c.ShareTime);
				if (pageIndex != 0 && pageSize != 0)
				{
					codes = codes.Skip(pageIndex * pageSize);
					codes = codes.Take(pageSize);
				}
				return codes.ToList();
			}).ConfigureAwait(false);
		}

		public async Task HandleAllUsersGiftCode()
		{
			var nowStamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
			var list = await GetAllValidGiftCodes(0, 100).ConfigureAwait(true);
			var handleUsers = context.GameR3Users.Where(u => u.Enable).Where(u => u.LastHandleStamp + u.HandleInterval < nowStamp).ToList();
			foreach (var u in handleUsers)
			{
				u.LastHandleStamp = nowStamp;
				context.GameR3Users.Update(u);

				var uAfter = await UpdateUser(u).ConfigureAwait(true);
				if (uAfter == null)
				{
					u.Enable = false;
					context.GameR3Users.Update(u);
					continue;
				}
				u.HandleInterval = new Random().Next(3600 * 1000, 3600 * 1000 * 24);//默认下次领取时间在1小时到1天之间
				var lastLogin = u.LastLogin;
				var comboTimes = u.LastSignIn?.ComboTimes ?? 0;
				if (lastLogin > new DateTime(0) && DateTime.Now.Day != (u.LastSignIn?.Date.Day ?? 0) || DateTime.Now.Subtract(lastLogin).TotalDays > comboTimes) continue;

				foreach (var c in list)
				{
					var cAfter = await HandleCode(u, c).ConfigureAwait(true);
					await Task.Delay(cAfter.Valid ? 500 : 1500).ConfigureAwait(true);
					if (!cAfter.Valid) break;
				}
			}
			await context.SaveChangesAsync().ConfigureAwait(true);
		}

		public async Task<IEnumerable<GainGiftCode>> GetUserGiftCodeHistory(User user)
		{
			return await Task.Run(() =>
			{
				var list = context.GainGiftCodeHistory.Where(h => h.User.GameId == user.GameId);
				return list.ToList();
			}).ConfigureAwait(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					context?.Dispose();
				}
				disposedValue = true;
			}
		}

		// // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
		// ~R3HandleServices()
		// {
		//     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
		//     Dispose(disposing: false);
		// }

		public void Dispose()
		{
			// 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}