using DAL.DTO.User;
using DAL.Entities;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.Settle;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Parsing.ExpressionVisitors.MemberBindings;
using System;
using System.Linq;

namespace BLL.Extensions
{
	public static class SettleExtensions
	{
		public static int GetYearlyVocation(this Settle settle, DbSet<AdminDivision> db)
		{
			var dis_lover = GetDistance(settle.Self.Address, settle.Lover.Address,db);
			var dis_parent = GetDistance(settle.Self.Address, settle.Parent.Address,db);
			var dis_l_p = GetDistance(settle.Lover.Address, settle.Parent.Address, db);
			return 0;//TODO 计算全年天数
		}
		private static int GetDistance(AdminDivision d1,AdminDivision d2, DbSet<AdminDivision> db)
		{
			if (d1.Code == d2.Code) return 0;
			return 1+Math.Min(
				d1==null?999:GetDistance(db.Where(x=>x.Code==d1.ParentCode).FirstOrDefault(),d2,db),
				d2==null?999:GetDistance(db.Where(x => x.Code == d2.ParentCode).FirstOrDefault(), d1, db));
		}

	}
}
