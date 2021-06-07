using BLL.Extensions;
using BLL.Interfaces;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainSchdule.Crontab;

namespace BLL.Crontab
{
    public class UserOrderJob : ICrontabJob
	{
		private readonly ApplicationDbContext context;

		public UserOrderJob(ApplicationDbContext context)
		{
			this.context = context;
		}

		/// <summary>
		/// 重新排序用户顺序
		/// </summary>
		public void Run()
		{
			var users = context.AppUsersDb.OrderByCompanyAndTitle().ToList();
			int index = 0;
			foreach(var u in users)
				u.UserOrderRank = long.MaxValue - index;
			context.AppUsers.UpdateRange(users);
			context.SaveChanges();
		}
	}
}
