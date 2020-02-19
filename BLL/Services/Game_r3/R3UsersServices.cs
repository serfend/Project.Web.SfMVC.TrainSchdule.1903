using BLL.Interfaces.GameR3;
using DAL.Data;
using DAL.Entities.Game_r3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.GameR3
{
	public class R3UsersServices : IR3UsersServices
	{
		private readonly ApplicationDbContext context;

		public R3UsersServices(ApplicationDbContext context)
		{
			this.context = context;
		}

		public async Task<IEnumerable<GainGiftCode>> GainGiftCodeHistory(string userid,string code, int pageIndex, int pageSize)
		{
			var history = context.GainGiftCodeHistory.AsQueryable();
			if (userid != null) history = history.Where(h => h.Code.Code == code);
			if (code != null) history = history.Where(h => h.User.GameId == userid);
			history=history.OrderByDescending(h => h.GainStamp).Skip(pageIndex * pageSize).Take(pageSize);
			return history.ToList();
		}

		public UserInfo GetUser(string userid)
		{
			return context.GameR3UserInfos.Where(u => u.User.GameId == userid).FirstOrDefault();
		}

		public async Task<IEnumerable<UserInfo>> Members(int pageIndex, int pageSize)
		{
			var users = context.GameR3UserInfos.OrderByDescending(u => u.DateTime).Skip(pageIndex * pageSize).Take(pageSize);
			return users.ToList();
		}
	}
}
