using DAL.DTO.User;
using DAL.Entities;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.Settle;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Parsing.ExpressionVisitors.MemberBindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Extensions
{
	public static class SettleExtensions
	{
		public static string CheckValid(this AdminDivision division, string alertName) => division == null ? $"{alertName}无效" : null;
		public static string CheckValid(this Moment moment, string alertName) => moment?.Address.CheckValid($"{alertName}地址");
		public static string AnyCodeInvalid(this Settle settle)
		{
			var result = settle == null ? "未设置家庭信息" : $"{settle.Self?.CheckValid("本人")}{settle.Lover?.CheckValid("配偶")}{settle.Parent?.CheckValid("父母")}{settle.LoversParent.CheckValid("岳父岳母")}";
			return result.Length == 0 ? null : result;
		}
		private class VacationUpdate
		{
			public int Month { get; set; }
			public int Day { get; set; }
			public string Description { get; set; }
		}
		/// <summary>
		/// 获取全年总假期，每年25日后，开始统计第二年休假
		/// </summary>
		/// <param name="settle"></param>
		/// <returns></returns>
		public static double GetYearlyLength(this Settle settle, User TargetUser, out VacationModefyRecord lastModefy, out VacationModefyRecord newModefy, out int maxOnTripTime, out string description)
		{
			var r = settle.GetYearlyLengthInner(TargetUser, out maxOnTripTime, out description);
			var record = settle?.PrevYealyLengthHistory;
			var modefyRecord = record.Where(rec=>rec.UpdateDate.Year==DateTime.Now.AddDays(5).Year).OrderByDescending(rec => rec.UpdateDate);
			lastModefy = modefyRecord.FirstOrDefault();
			newModefy = null;
			if (lastModefy == null)
			{
				newModefy = new VacationModefyRecord()
				{
					UpdateDate = TargetUser.CheckUpdateDate(),
					Length = r
				};
				return r;
			}
			else if (lastModefy.Length != r)
			{
				//【政策】如果是今年调整的，那么按比例计算
				var modefyDate = lastModefy.UpdateDate;
				var newr = (int)(((12 - modefyDate.Month) * r + modefyDate.Month * lastModefy.Length) / 12);
				// TODO 后续可能需要减去事假天数 newr -= settle?.PrevYearlyComsumeLength ?? 0;
				newModefy = new VacationModefyRecord()
				{
					Length = newr,
					UpdateDate = TargetUser.CheckUpdateDate()
				};
				var tmpDesc = new StringBuilder(description);
				tmpDesc.Append( $" 假期全年发生{modefyRecord.Count()}次变动，根据政策规定，新的假期为 变动前天数×变动月份÷12 + 变动后天数×(12-变动月份)÷12。");
				int nowCount = 0,maxCount =modefyRecord.Count();
				foreach(var m in modefyRecord)
				{
					tmpDesc.Append($"[{++nowCount}]{m.UpdateDate.Month}月{m.UpdateDate.Day}日，假期天数为{(int)m.Length}");
					tmpDesc.Append(nowCount == maxCount ? '。' : '；');
				}
				r = newr;
			}
			return r;
		}
		/// <summary>
		/// 检查最后一次更新的时间
		/// </summary>
		/// <param name="TargetUser"></param>
		/// <returns></returns>
		public static DateTime CheckUpdateDate(this User TargetUser)
		{
			var maxDate = DateTime.MinValue;
			if (TargetUser == null) return maxDate;
			if (maxDate < TargetUser.SocialInfo?.Settle?.Self?.Date) maxDate = TargetUser.SocialInfo?.Settle?.Self?.Date ?? maxDate;
			if (maxDate < TargetUser.SocialInfo?.Settle?.Parent?.Date) maxDate = TargetUser.SocialInfo?.Settle?.Parent?.Date ?? maxDate;
			if (maxDate < TargetUser.SocialInfo?.Settle?.Lover?.Date) maxDate = TargetUser.SocialInfo?.Settle?.Lover?.Date ?? maxDate;
			if (maxDate < TargetUser.SocialInfo?.Settle?.LoversParent?.Date) maxDate = TargetUser.SocialInfo?.Settle?.LoversParent?.Date ?? maxDate;
			if (maxDate < TargetUser.CompanyInfo.TitleDate) maxDate = TargetUser.CompanyInfo.TitleDate ?? maxDate;
			return maxDate;
		}
		public static int GetYearlyLengthInner(this Settle settle, User targetUser, out int maxOnTripTime, out string description)
		{
			maxOnTripTime = 1;
			description = "工作满20年，驻地假30天。";
			var workYears = SystemNowDate().Year - targetUser?.BaseInfo.Time_Work.Year + 1;
			if (workYears > 20 || (workYears == 20 && SystemNowDate().Month >= targetUser?.BaseInfo.Time_Work.Month)) return 30;
			var title = targetUser.CompanyInfo.Title;
			if (title != null && title.VacationDay != 0)
			{
				description = $"{title.Name}，假期天数{title.VacationDay}天";
				return title.VacationDay;
			}
			description = "未婚，探父母假30天。";
			if (settle?.Lover == null) return 30;
			var dis_lover = IsAllopatry(settle.Self?.Address, settle.Lover?.Address);//与配偶不在一地
			var dis_parent = IsAllopatry(settle.Self?.Address, settle.Parent?.Address);//与自己的家长不在一地
			var dis_l_p = IsAllopatry(settle.Lover?.Address, settle.Parent?.Address) || IsAllopatry(settle.LoversParent?.Address, settle.Lover?.Address);//配偶与任意一方家长不在一地
			description = "已婚且与妻子同地，探父母假20天。";
			if (!dis_lover) return 20;

			maxOnTripTime = 3;
			description = "已婚且三方异地，探父母假、探配偶假共计45天。";
			if (dis_parent && dis_l_p) return 45;
			maxOnTripTime = 2;
			description = "已婚且父母、妻子异地但双方父母皆与妻子不异地，探父母假、探配偶假共计40天。";
			return 40;
		}
		private static bool IsAllopatry(AdminDivision d1, AdminDivision d2)
		{
			if (d1 == null || d2 == null) return false;
			var codeCity1 = (int)(d1.Code / 100d);
			var codeCity2 =(int)(d2.Code / 100d);

			return codeCity1 != codeCity2;
		}
		private static int GetDistance(AdminDivision d1, AdminDivision d2, DbSet<AdminDivision> db)
		{
			if (d1.Code == d2.Code) return 0;
			return 1 + Math.Min(
				d1 == null ? 999 : GetDistance(db.Where(x => x.Code == d1.ParentCode).FirstOrDefault(), d2, db),
				d2 == null ? 999 : GetDistance(db.Where(x => x.Code == d2.ParentCode).FirstOrDefault(), d1, db));
		}
		public static DateTime SystemNowDate()
		{
			return DateTime.Now.AddDays(5);
		}
	}
}
