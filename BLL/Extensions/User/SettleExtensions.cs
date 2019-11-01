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
		/// <summary>
		/// 获取全年总假期
		/// </summary>
		/// <param name="settle"></param>
		/// <returns></returns>
		public static int GetYearlyLength(this Settle settle,out int maxOnTripTime)
		{
			maxOnTripTime = 1;
			if (settle.Lover == null) return 30;
			var dis_lover = IsAllopatry(settle.Self.Address, settle.Lover.Address);
			var dis_parent = IsAllopatry(settle.Self.Address, settle.Parent.Address);
			var dis_l_p = IsAllopatry(settle.Lover.Address, settle.Parent.Address);
			if (!dis_lover) return 20;
			maxOnTripTime = 3;
			if (!dis_parent && !dis_l_p) return 45;
			maxOnTripTime = 2;
			return 40;
		}
		private static bool IsAllopatry(AdminDivision d1,AdminDivision d2)
		{
			if (d1 == null || d2 == null) return false;
			return d1.Code / 100d != d2.Code / 100d; 
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
