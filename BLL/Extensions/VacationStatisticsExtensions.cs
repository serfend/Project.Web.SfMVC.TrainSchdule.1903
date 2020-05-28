using DAL.Data;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using DAL.Entities.Vacations;
using DAL.Entities.Vacations.VacationsStatistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Extensions
{
	public static class VacationStatisticsExtensions
	{
		public static void InitStatisticsInit(this VacationStatisticsDescription model, ApplicationDbContext context, int currentYear, string statisticsId)
		{
			if (context == null) return;
			if (model == null) return;
			model.StatisticsId = statisticsId;
			foreach (var m in model.Childs)
				m.InitStatisticsInit(context, currentYear, statisticsId);
			model.InitCurrent(context);
		}

		private static void InitCurrent(this VacationStatisticsDescription model, ApplicationDbContext context)
		{
			var current = model.Data;
			var applies = model.Applies;
			// 此处应按职务等级类型创建多个
			var cur = new VacationStatisticsDescriptionData();
			cur.ApplyCount = applies.Count();
			cur.ApplyMembersCount = applies.Select(a => a.BaseInfo.From.Application.Id).Distinct().Count();
			cur.ApplySumDayCount = applies.Select(a => a.RequestInfo).Where(r => r.StampLeave.HasValue).Where(r => r.StampReturn.HasValue).Sum(r => (int)(r.StampReturn.Value.Subtract(r.StampLeave.Value).TotalDays + 1));
			cur.CompleteVacationExpectDayCount = applies.Sum(a => (int)a.BaseInfo.From.SocialInfo.Settle.GetYearlyLength(a.BaseInfo.From).Item1);
			var recallApplies = context.RecallOrders.Where(r => applies.Where(a => a.RecallId != null).Select(a => a.Id).Contains(r.Id));
			// 休假天数-召回天数
			cur.CompleteVacationRealDayCount = cur.ApplySumDayCount - recallApplies.Sum(r => (int)applies.FirstOrDefault(a => a.RecallId == r.Id).RequestInfo.StampLeave.Value.Subtract(r.ReturnStramp).TotalDays);
		}
	}
}