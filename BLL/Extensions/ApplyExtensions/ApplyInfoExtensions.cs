using BLL.Extensions.ApplyExtensions.ApplyAuditStreamExtension;
using BLL.Extensions.Common;
using DAL.Data;
using DAL.DTO.Apply;
using DAL.DTO.User;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.Vacations.Statistics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Extensions.ApplyExtensions
{
	public static class ApplyInfoExtensions
	{
		public static ApplyDetailDto ToDetaiDto(this Apply model, UserVacationInfoVDto info)
		{
			if (model == null) return null;
			var b = new ApplyDetailDto()
			{
				Base = model.BaseInfo.From.ToSummaryDto(),
				Company = model.BaseInfo.Company,
				Create = model.Create,
				Duties = model.BaseInfo.Duties,
				Hidden = model.Hidden,
				RequestInfo = model.RequestInfo,
				Response = model.Response.Select(r => r.ToResponseDto()),
				NowStep = model?.NowAuditStep?.ToDtoModel(),
				Steps = model?.ApplyAllAuditStep?.Select(a => a.ToDtoModel()),
				AuditSolution = model?.ApplyAuditStreamSolutionRule?.Solution?.Name ?? "旧版本方式审批",
				Id = model.Id,
				Social = model.BaseInfo.Social,
				Status = model.Status,
				AuditLeader = model.AuditLeader,
				RecallId = model.RecallId,
				UserVacationDescription = info
			};
			return b;
		}

		public static ApplySummaryDto ToSummaryDto(this Apply model)
		{
			var b = new ApplySummaryDto()
			{
				Create = model?.Create,
				Status = model.Status,
				Base = model.BaseInfo.ToDto(),
				UserBase = model.BaseInfo.From.ToSummaryDto(),
				Id = model.Id,
				Request = model.RequestInfo,
				RecallId = model.RecallId,
				NowStep = model?.NowAuditStep?.ToDtoModel(),
				Steps = model?.ApplyAllAuditStep?.Select(a => a.ToDtoModel()).OrderBy(l => l.Index),
				AuditStreamSolution = model?.ApplyAuditStreamSolutionRule?.Solution?.Name ?? "未知的审批方式"
			};
			return b;
		}

		/// <summary>
		/// 获取当前统计单位所有已通过的假期
		/// </summary>
		/// <param name="companyCode"></param>
		/// <returns></returns>
		public static IQueryable<Apply> GetCurrentApplies(this IQueryable<Apply> db, string companyCode)
		{
			if (companyCode == null) return db.Where(a => false);
			var codeLen = companyCode.Length;
			return db.Where(a => a.Status == AuditStatus.Accept)
				.Where(s => s.BaseInfo.Company.Code.Length >= codeLen && s.BaseInfo.Company.Code.Substring(0, codeLen) == companyCode);
		}

		/// <summary>
		/// 获取统计库，以便于检查已统计过的缓存
		/// 使用目标日期当天的数据
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="companyCode"></param>
		/// <param name="target"></param>
		/// <param name="db"></param>
		/// <returns></returns>
		public static IQueryable<T> CheckDb<T>(this IQueryable<T> db, string companyCode, DateTime target) where T : IStatisticsBase => db.CheckDb(companyCode, target, target.LastMilliSecondInDay());

		/// <summary>
		/// 获取统计库，以便于检查已统计过的缓存
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="companyCode"></param>
		/// <param name="vStart"></param>
		/// <param name="vEnd"></param>
		/// <param name="db"></param>
		/// <returns></returns>
		public static IQueryable<T> CheckDb<T>(this IQueryable<T> db, string companyCode, DateTime vStart, DateTime vEnd) where T : IStatisticsBase
		{
			if (companyCode == null) return db.Where(a => false);
			return db.Where(s => s.Target >= vStart)
				.Where(s => s.Target <= vEnd)
				.Where(s => s.CompanyCode == companyCode);
		}

		/// <summary>
		/// 计算统计，将从vStart到vEnd逐日进行统计
		/// 当统计天数大于1200天时，则缩短至1200天
		/// </summary>
		/// <typeparam name="T">实现<see cref="IStatisticsBase"/>的实例</typeparam>
		/// <param name="db">通过此db去判断是否已经统计过</param>
		/// <param name="context">数据库聚合</param>
		/// <param name="GetTargetStatistics">定义如何获取目标统计结果以及是否需要添加到数据库</param>
		/// <param name="GetTargetAppliesByDay">如何通过日期获取指定假期申请</param>
		/// <param name="GetEachEndDay">如何通过vStart,vEnd及pDate获取当日界定范围（pDate为当日开始日期）</param>
		/// <param name="SaveToDb">定义如何保存到数据库</param>
		/// <param name="companyCode">需要查询的单位</param>
		/// <param name="vStart">统计开始时间</param>
		/// <param name="vEnd">统计结束时间</param>
		/// <returns></returns>
		public static IEnumerable<T> CaculateIStatisticsBaseApplies<T>(
			this IQueryable<T> db, ApplicationDbContext context,
			Func<string, DateTime, IQueryable<T>, IQueryable<Apply>, IQueryable<RecallOrder>, Tuple<IEnumerable<T>, bool>> GetTargetStatistics,
			Func<IQueryable<Apply>, IQueryable<RecallOrder>, DateTime, DateTime, IQueryable<Apply>> GetTargetAppliesByDay,
			Func<DateTime, DateTime, DateTime, DateTime> GetEachEndDay,
			Action<IEnumerable<T>> SaveToDb,
			string companyCode, DateTime vStart, DateTime vEnd) where T : IStatisticsBase, new()
		{
			if (companyCode == null || context == null) return new List<T>();
			vEnd = vEnd.EndDateNotEarlyThanNow();
			if (vEnd.Subtract(vStart).TotalDays > 1200) vStart = vEnd.Subtract(TimeSpan.FromDays(1200));
			var recallDb = context.RecallOrders;

			var pDate = vStart.Date;
			var result = new List<T>();
			bool hasChange = false;
			var applies = context.AppliesDb.GetCurrentApplies(companyCode);
			while (pDate < vEnd)
			{
				var pEnd = GetEachEndDay(pDate, vStart, vEnd);
				var earlyDate = pDate > pEnd ? pEnd : pDate;
				var laterDate = pDate < pEnd ? pEnd : pDate;
				var thisDayApplies = GetTargetAppliesByDay.Invoke(applies, recallDb, earlyDate, laterDate);
				var r = GetTargetStatistics.Invoke(companyCode, pDate, db, thisDayApplies, recallDb);
				result.AddRange(r.Item1);
				if (r.Item2)
				{
					SaveToDb.Invoke(r.Item1);
					hasChange = true;
				}
				pDate = pDate.AddDays(1);
			}
			if (hasChange) context.SaveChanges();
			return result;
		}

		/// <summary>
		/// 获取一天的最后一毫秒
		/// </summary>
		/// <param name="pDate">哪一天</param>
		/// <param name="vStart">无效项</param>
		/// <param name="vEnd">无效项</param>
		/// <returns></returns>
		public static DateTime LastMilliSecondInDay(this DateTime pDate, DateTime vStart, DateTime vEnd) => pDate.LastMilliSecondInDay();

		/// <summary>
		/// 获取一天的公元新年第一天
		/// </summary>
		/// <param name="pDate">哪一天</param>
		/// <param name="vStart">无效项</param>
		/// <param name="vEnd">无效项</param>
		/// <returns></returns>
		public static DateTime FirstYearOfTheDay(this DateTime pDate, DateTime vStart, DateTime vEnd) => pDate.FirstYearOfTheDay();

		/// <summary>
		///  获取新增的假期
		/// </summary>
		/// <param name="applies"></param>
		/// <param name="recallDb"></param>
		/// <param name="pDate"></param>
		/// <param name="pDateEnd"></param>
		/// <returns></returns>
		public static IQueryable<Apply> GetNewApplies(this IQueryable<Apply> applies, IQueryable<RecallOrder> recallDb, DateTime pDate, DateTime pDateEnd) => applies.Where(a => a.RequestInfo.StampLeave.HasValue)
				.Where(a => a.RequestInfo.StampLeave.Value >= pDate)
				.Where(a => a.RequestInfo.StampLeave.Value <= pDateEnd);

		/// <summary>
		///  获取已完成的假期，包括正常完成的假期和因为召回而提前完成的假期
		/// </summary>
		/// <param name="applies"></param>
		/// <param name="recallDb"></param>
		/// <param name="pDate"></param>
		/// <param name="pDateEnd"></param>
		/// <returns></returns>
		public static IQueryable<Apply> GetCompletedApplies(this IQueryable<Apply> applies, IQueryable<RecallOrder> recallDb, DateTime pDate, DateTime pDateEnd)
		=> applies.Where(
				a =>
				(a.RecallId != null
				&& recallDb.Where(rec => rec.Id == a.RecallId).First().ReturnStramp >= pDate
				&& recallDb.Where(rec => rec.Id == a.RecallId).First().ReturnStramp <= pDateEnd)
				||
				(a.RecallId == null
				&& a.RequestInfo.StampReturn.HasValue
				&& a.RequestInfo.StampReturn.Value >= pDate
				&& a.RequestInfo.StampReturn.Value <= pDateEnd)
				);
	}
}