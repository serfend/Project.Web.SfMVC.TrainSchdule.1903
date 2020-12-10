using BLL.Interfaces.BBS;
using DAL.Data;
using DAL.Entities.BBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Services.BBS
{
	public class SignInServices : ISignInServices
	{
		private readonly ApplicationDbContext context;

		public SignInServices(ApplicationDbContext context)
		{
			this.context = context;
		}

		private IQueryable<SignIn> Query(string signId, DateTime? startDate, DateTime? endDate)
		{
			var list = context.SignIns.AsQueryable();
			if (signId != null) list = list.Where(s => s.SignId == signId);
			if (startDate != null && endDate != null) list = list.Where(s => s.Date >= startDate).Where(s => s.Date <= endDate);
			return list;
		}

		public IEnumerable<SignIn> QueryByDate(string signId, DateTime startDate, DateTime endDate)
		{
			return Query(signId, startDate, endDate).ToList();
		}

		public SignIn QuerySingle(string signId, DateTime startDate, DateTime endDate)
		{
			return Query(signId, startDate, endDate).FirstOrDefault();
		}

		public void SignIn(string signId)
		{
			var lastDaySignIn = QuerySingle(signId, DateTime.Today.AddDays(-1), DateTime.Today);
			context.SignIns.Add(new DAL.Entities.BBS.SignIn()
			{
				Date = DateTime.Now,
				SignId = signId,
				ComboTimes = lastDaySignIn?.ComboTimes ?? 0 + 1
			});
			context.SaveChanges();
		}
	}
}