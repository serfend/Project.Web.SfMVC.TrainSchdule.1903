using BLL.Interfaces.IVacationStatistics;
using DAL.Data;
using DAL.Entities.ApplyInfo;
using DAL.Entities.Vacations.Statistics.StatisticsNewApply;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Services.VacationStatistics
{
	public class StatisrticsAppliesServices : IStatisrticsAppliesServices
	{
		private readonly ApplicationDbContext _context;

		public StatisrticsAppliesServices(ApplicationDbContext context)
		{
			_context = context;
		}

		public IEnumerable<StatisticsApplyComplete> CaculateCompleteApplies(string companyCode, DateTime vStart, DateTime vEnd)
		{
			if (companyCode == null) return new List<StatisticsApplyComplete>();
			vEnd = EndDateNotEarlyThanNow(vEnd);
			var db = _context.StatisticsCompleteApplies.AsQueryable();
			var recallDb = _context.RecallOrders;

			var pDate = vStart.Date;
			var result = new List<StatisticsApplyComplete>();
			bool hasChange = false;
			var applies = GetCurrentApplies(companyCode);
			while (pDate < vEnd)
			{
				var pDateEnd = pDate.AddDays(1).AddMilliseconds(-1);
				var thisDayApplies = applies.Where(
				a =>
				(a.RecallId != null
				&& recallDb.Where(rec => rec.Id == a.Id).First().ReturnStramp >= pDate
				&& recallDb.Where(rec => rec.Id == a.Id).First().ReturnStramp <= pDateEnd)
				||
				(a.RecallId == null
				&& a.RequestInfo.StampReturn.HasValue
				&& a.RequestInfo.StampReturn.Value >= pDate
				&& a.RequestInfo.StampReturn.Value <= pDateEnd)
				);
				var r = GetTargetStatistics(companyCode, pDate, db, thisDayApplies);
				result.AddRange(r.Item1);
				if (r.Item2)
				{
					_context.StatisticsCompleteApplies.AddRange(r.Item1);
					hasChange = true;
				}
				pDate = pDate.AddDays(1);
			}
			if (hasChange) _context.SaveChanges();
			return result;
		}

		public IEnumerable<StatisticsApplyNew> CaculateNewApplies(string companyCode, DateTime vStart, DateTime vEnd)
		{
			if (companyCode == null) return new List<StatisticsApplyNew>();
			vEnd = EndDateNotEarlyThanNow(vEnd);
			var db = _context.StatisticsNewApplies.AsQueryable();
			var applies = GetCurrentApplies(companyCode);

			var pDate = vStart.Date;
			var result = new List<StatisticsApplyNew>();
			bool hasChange = false;
			while (pDate < vEnd)
			{
				var pDateEnd = pDate.AddDays(1).AddMilliseconds(-1);
				var thisDayApplies = applies.Where(a => a.RequestInfo.StampLeave.HasValue).Where(a => a.RequestInfo.StampLeave.Value >= pDate).Where(a => a.RequestInfo.StampLeave.Value <= pDateEnd);
				var r = GetTargetStatistics(companyCode, pDate, db, thisDayApplies);
				result.AddRange(r.Item1);
				if (r.Item2)
				{
					_context.StatisticsNewApplies.AddRange(r.Item1);
					hasChange = true;
				}
				pDate = pDate.AddDays(1);
			}
			if (hasChange) _context.SaveChanges();
			return result;
		}

		/// <summary>
		/// 获取当前统计单位所有已通过的假期
		/// </summary>
		/// <param name="companyCode"></param>
		/// <returns></returns>
		private IQueryable<Apply> GetCurrentApplies(string companyCode)
		{
			var codeLen = companyCode.Length;
			return _context.AppliesDb.Where(a => a.Status == AuditStatus.Accept).Where(s => s.BaseInfo.Company.Code.Substring(0, codeLen) == companyCode); ;
		}

		/// <summary>
		/// 获取统计库，以便于检查已统计过的缓存
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="companyCode"></param>
		/// <param name="vStart"></param>
		/// <param name="vEnd"></param>
		/// <param name="db"></param>
		/// <returns></returns>
		private static IQueryable<T> CheckDb<T>(string companyCode, DateTime target, IQueryable<T> db) where T : IStatisticsApplyBase
		{
			var vStart = target.Date;
			var vEnd = target.Date.AddDays(1).AddMilliseconds(-1);
			var codeLen = companyCode.Length;
			return db.Where(s => s.Target >= vStart)
				.Where(s => s.Target <= vEnd)
				.Where(s => s.CompanyCode.Substring(0, codeLen) == companyCode);
		}

		/// <summary>
		/// 进行统计
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="companyCode"></param>
		/// <param name="target"></param>
		/// <param name="db">目标日的申请</param>
		/// <returns>返回是否需要添加到数据库</returns>
		private Tuple<IEnumerable<T>, bool> GetTargetStatistics<T>(string companyCode, DateTime target, IQueryable<T> db, IQueryable<Apply> applies) where T : IStatisticsApplyBase, new()
		{
			if (CheckDb(companyCode, target, db).Any()) return new Tuple<IEnumerable<T>, bool>(CheckDb(companyCode, target, db), false);
			var records = applies.Select(a => new T()
			{
				From = (byte)(a.BaseInfo.From.SocialInfo.Settle.Self.Address.Code / 10000),
				To = (byte)(a.RequestInfo.VacationPlace.Code / 10000),
				CompanyCode = companyCode,
				Target = target,
				Type = a.BaseInfo.From.CompanyInfo.Duties.Type,
				Day = a.RequestInfo.VacationLength // 此处可能需要仅计算正休假
			});
			var groupRecords = records.GroupBy(a => new { a.From, a.To, a.CompanyCode, a.Target, a.Type });
			var result = groupRecords.ToList().Select(r => new T()
			{
				Target = r.Key.Target,
				To = r.Key.To,
				From = r.Key.From,
				Type = r.Key.Type,
				CompanyCode = r.Key.CompanyCode,
				Value = r.Count(),
				Day = r.Sum(a => a.Day)
			});
			return new Tuple<IEnumerable<T>, bool>(result, true);
		}

		/// <summary>
		/// 统计结束时间不可晚于今日
		/// </summary>
		/// <param name="vEnd"></param>
		/// <returns></returns>
		private static DateTime EndDateNotEarlyThanNow(DateTime vEnd) => vEnd >= DateTime.Today ? DateTime.Today.AddSeconds(-1) : vEnd;
	}
}