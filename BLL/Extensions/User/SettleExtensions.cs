﻿using DAL.Entities;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.Settle;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Extensions
{
	public static class SettleExtensions
	{
		public static string CheckValid(this AdminDivision division, string alertName) => division == null ? $"{alertName}无效" : null;

		public static string CheckValid(this Moment moment, string alertName)
		{
			var info = moment?.Address.CheckValid($"{alertName}地址");
			if (info != null) return info;
			if (moment.Valid && moment.Address.Code == 0) return $"{alertName}需要填写的市县";
			return null;
		}

		public static string AnyCodeInvalid(this Settle settle)
		{
			var result = settle == null ? "未设置家庭信息" : $"{settle.Self?.CheckValid("本人")}{settle.Lover?.CheckValid("配偶")}{settle.Parent?.CheckValid("父母")}{settle.LoversParent.CheckValid("岳父岳母")}";
			return result.Length == 0 ? null : result;
		}

		/// <summary>
		/// 加权计算新长度  新长度=老长度*新长度月份+新长度*(12-新长度月份)  /12
		/// </summary>
		/// <param name="records"></param>
		/// <param name="newDate"></param>
		/// <param name="newLength"></param>
		/// <param name="weightDescription"></param>
		/// <returns></returns>
		public static double CaculateLengthByWeight(this IEnumerable<AppUsersSettleModefyRecord> records, DateTime newDate, double newLength, out string weightDescription)
		{
			weightDescription = "";
			var lastestModefy = records?.FirstOrDefault();
			if (lastestModefy == null || lastestModefy.UpdateDate.Year < newDate.Year) return newLength;

			var result = ((12 - newDate.Month) * newLength + newDate.Month * lastestModefy.Length) / 12;
			var weightDescriptionB = new StringBuilder();
			weightDescriptionB.Append($"因今年发生{records.Count()}次变化，全年假期应加权计算：");
			weightDescriptionB.Append($"新长度=(老长度*新长度月份+新长度*(12-新长度月份) ) /12=({(int)lastestModefy.Length}*{newDate.Month}+{(int)newLength}*(12-{newDate.Month}))={(int)(result)}");
			weightDescription = weightDescriptionB.ToString();
			return result;
		}

		/// <summary>
		///  获取全年总假期，每年12月25日后，开始统计第二年休假
		/// </summary>
		/// <param name="settle"></param>
		/// <param name="TargetUser"></param>
		/// <returns>
		/// double 结果
		/// <param name="requireToAdd">当需要添加记录时返回非空</param>
		/// <param name="maxOnTripTime">全年可休几次路途</param>
		/// <param name="description">全年假期的描述</param>
		/// </returns>
		public static Tuple<double, AppUsersSettleModefyRecord, int, string> GetYearlyLength(this Settle settle, User TargetUser)
		{
			if (settle == null) throw new ArgumentNullException(nameof(settle));
			AppUsersSettleModefyRecord requireToAdd;
			var nowVacationLength = settle.GetYearlyLengthInner(TargetUser, out int maxOnTripTime, out string description); // 本次应休假长度
			var userFinnalModefyDate = TargetUser.CheckUpdateDate(); // 本次用户家庭最后变更时间
			var vacationModefyRecords = settle.PrevYealyLengthHistory?.OrderByDescending(rec => rec.UpdateDate); // 用户假期变更记录
			var lastVacationModefy = vacationModefyRecords?.FirstOrDefault(); // 上次假期变更记录
			var lastFamilyVacationModefy = vacationModefyRecords?.FirstOrDefault(rec => !rec.IsNewYearInitData); // 上次因家庭变更而假期变更记录

			// 通过用户家庭情况变更记录来计算

			// 如果今年已有变更记录
			if (lastVacationModefy != null && lastVacationModefy.UpdateDate.Year == DateTime.Now.XjxtNow().Year)
			{
				// 本次假期长度同上次，则无更改
				if (lastVacationModefy.Length == nowVacationLength)
				{
					description = lastVacationModefy.Description;
					requireToAdd = null;
					return new Tuple<double, AppUsersSettleModefyRecord, int, string>(nowVacationLength, requireToAdd, maxOnTripTime, description);
				}
				// 上次是否是新年初始化 或 上次的家庭情况时间不同于本次
				else if (lastVacationModefy.IsNewYearInitData || lastVacationModefy.UpdateDate != userFinnalModefyDate)
				{
					var thisYearModefyRecords = vacationModefyRecords.Where(rec => rec.UpdateDate.Year == DateTime.Now.XjxtNow().Year); // 今年以来的变更记录
					var newLength = thisYearModefyRecords.CaculateLengthByWeight(userFinnalModefyDate, nowVacationLength, out var weightDescription);

					requireToAdd = new AppUsersSettleModefyRecord()
					{
						Description = $"{description} {weightDescription}",
						IsNewYearInitData = false,
						Length = newLength,
						UpdateDate = userFinnalModefyDate
					};
					return new Tuple<double, AppUsersSettleModefyRecord, int, string>(newLength, requireToAdd, maxOnTripTime, description);
				}
				// 否则只是因为加权导致的天数不同而已，返回上次的值即可
				else
				{
					requireToAdd = null;
					description = lastVacationModefy.Description;
					return new Tuple<double, AppUsersSettleModefyRecord, int, string>(lastVacationModefy.Length, requireToAdd, maxOnTripTime, description);
				}
			}
			// 若今年无变更记录，则创建一条记录
			else
			{
				requireToAdd = new AppUsersSettleModefyRecord()
				{
					Description = description,
					IsNewYearInitData = true,
					UpdateDate = new DateTime(DateTime.Now.XjxtNow().Year, 1, 1),
					Length = nowVacationLength
				};
				return new Tuple<double, AppUsersSettleModefyRecord, int, string>(nowVacationLength, requireToAdd, maxOnTripTime, description);
			}
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

		/// <summary>
		/// 根据用户条件获取当前用户的全年总假期长度
		/// </summary>
		/// <param name="settle"></param>
		/// <param name="targetUser"></param>
		/// <param name="maxOnTripTime"></param>
		/// <param name="description"></param>
		/// <returns></returns>
		public static int GetYearlyLengthInner(this Settle settle, User targetUser, out int maxOnTripTime, out string description)
		{
			maxOnTripTime = 0;
			description = "本人地址无效，无休假";
			if (targetUser == null || settle?.Self == null || (!settle.Self?.Valid ?? false)) return 0;

			if (settle?.Lover == null || (!settle.Lover?.Valid ?? false))
			{
				var title = targetUser.CompanyInfo.Title;
				if (title != null && title.VacationDay != 0)
				{
					maxOnTripTime = 1;
					description = $"未婚，且职务为{title.Name}，假期天数{title.VacationDay}天";
					return title.VacationDay;
				}
				description = "未婚，探父母假30天。";
				maxOnTripTime = 1;
				return 30;
			}

			var dis_lover = IsAllopatry(settle.Self, settle.Lover);//与配偶不在一地
			var dis_parent = IsAllopatry(settle.Self, settle.Parent); // 与自己的家长不在一地
			var dis_l_p = IsAllopatry(settle.Lover, settle.Parent) || IsAllopatry(settle.LoversParent, settle.Lover);//配偶与任意一方家长不在一地

			if (dis_lover && dis_parent && dis_l_p)
			{
				maxOnTripTime = 3;
				description = "已婚且三方异地，探父母假、探配偶假共计45天。"; return 45;
			}

			if (dis_lover)
			{
				maxOnTripTime = 2;
				description = "已婚两方异地，探父母假、探配偶假共计40天。"; return 40;
			}

			var workYears = SystemNowDate().Year - targetUser?.BaseInfo.Time_Work.Year + 1;
			if (workYears > 20 || (workYears == 20 && SystemNowDate().Month >= targetUser?.BaseInfo.Time_Work.Month))
			{
				maxOnTripTime = 0;
				description = "工作满20年，驻地假30天。"; return 30;
			}

			if (!dis_lover)
			{
				if (dis_l_p)
				{
					maxOnTripTime = 1;
					description = "已婚且与妻子同地，与父母异地，探父母假计20天。"; return 20;
				}
				else
				{
					maxOnTripTime = 0;
					description = "已婚且与妻子、父母同地，驻地假计20天。"; return 20;
				}
			}

			description = "异常的个人信息，请核实";
			return 0;
		}

		private static bool IsAllopatry(Moment d1, Moment d2)
		{
			var a1 = d1?.Address;
			var a2 = d2?.Address;
			if (a1 == null || a2 == null || !d1.Valid || !d2.Valid) return false;
			var codeCity1 = (int)(a1.Code / 100d);
			var codeCity2 = (int)(a2.Code / 100d);

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