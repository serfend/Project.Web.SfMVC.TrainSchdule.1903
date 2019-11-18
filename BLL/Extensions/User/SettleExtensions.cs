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
		public static int GetYearlyLength(this Settle settle,User TargetUser,out int maxOnTripTime,out string description)
		{
			var r = settle.GetYearlyLengthInner(TargetUser,out maxOnTripTime,out description);
			if (settle?.PrevYearlyLength != r)
			{
				var modefyDate = settle?.Lover?.Date;

				//如果是今年调整的，那么按比例计算
				//TODO 可能有不是因为结婚导致的变化，需要提前考虑
				if (modefyDate?.Year == DateTime.Today.Year)
				{
					var newr = ((12 - modefyDate?.Month) * r + modefyDate?.Month * settle.PrevYearlyLength) / 12;
					description += $"\n年初全年总假{settle.PrevYearlyLength}天，因{modefyDate?.Month}月发生变化，按比例加权:(12-变化的月) * 变化后天数 + 变化的月 * 年初总假期=（{12 - modefyDate?.Month} * {r } + { modefyDate?.Month} * {settle.PrevYearlyLength}）/12={newr}。";
					r = newr??0;
				}
			}
			return r;
		}
		public static int GetYearlyLengthInner(this Settle settle,User targetUser,out int maxOnTripTime,out string description)
		{
			maxOnTripTime = 1;
			description = "工作满20年，正常驻地假30天。";
			if (DateTime.Today.Year - targetUser?.BaseInfo.Time_Work.Year + 1 >= 20) return 30;
            description="未婚，正常驻地假30天。";
			if (settle?.Lover == null) return 30;
			var dis_lover = IsAllopatry(settle.Self.Address, settle.Lover.Address);//与配偶不在一地
			var dis_parent = IsAllopatry(settle.Self.Address, settle.Parent.Address);//与自己的家长不在一地
			var dis_l_p = IsAllopatry(settle.Lover.Address, settle.Parent.Address)||IsAllopatry(settle.LoversParent.Address,settle.Lover.Address);//配偶与任意一方家长不在一地
			description = "已婚且与妻子同地，探父母假20天。";
			if (!dis_lover)return 20;
			
			maxOnTripTime = 3;
			description = "已婚且与三方异地，基础假30天，探配偶假10天，探父母假5天，合计45天。";
			if (!dis_parent && !dis_l_p) return 45;
			maxOnTripTime = 2;
			description = "已婚且与父母、妻子异地但父母与妻子不异地，基础假30天，探配偶假10天，合计40天。";
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
