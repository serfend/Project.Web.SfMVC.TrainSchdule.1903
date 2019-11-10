using BLL.Extensions;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.Crontab
{
	/// <summary>
	/// 每年1月1日重置所有人当年假期
	/// </summary>
	public class NewYearVocationUpdateJob:ICrontabJob
	{
		private readonly ApplicationDbContext _context;

		public NewYearVocationUpdateJob(ApplicationDbContext context)
		{
			_context = context;
		}

		public void Run()
		{
			var allUsers = _context.AppUsers.ToList();
			foreach(var u in allUsers)
			{
				u.SocialInfo.Settle.PrevYearlyLength = u.SocialInfo.Settle.GetYearlyLengthInner(u, out var i, out var j);
			}
		}
	}
}
