using BLL.Extensions.ApplyExtensions;
using BLL.Extensions.Common;
using BLL.Interfaces.IVacationStatistics;
using DAL.Data;
using DAL.Entities;
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

		public IEnumerable<StatisticsApplyComplete> CaculateCompleteApplies(string companyCode, DateTime vStart, DateTime vEnd) => _context.StatisticsCompleteApplies.AsQueryable()
			.CaculateIStatisticsBaseApplies(_context, GetTargetStatistics, ApplyInfoExtensions.GetCompletedApplies, ApplyInfoExtensions.LastMilliSecondInDay
			, item => _context.StatisticsCompleteApplies.AddRange(item), companyCode, vStart, vEnd);

		public IEnumerable<StatisticsApplyNew> CaculateNewApplies(string companyCode, DateTime vStart, DateTime vEnd) =>
			_context.StatisticsNewApplies.AsQueryable()
			.CaculateIStatisticsBaseApplies(_context, GetTargetStatistics, ApplyInfoExtensions.GetNewApplies, ApplyInfoExtensions.LastMilliSecondInDay
				, item => _context.StatisticsNewApplies.AddRange(item), companyCode, vStart, vEnd);

		public void RemoveCompleteApplies(string companyCode, DateTime vStart, DateTime vEnd)
		{
			var list = _context.StatisticsCompleteApplies.Where(s => s.CompanyCode == companyCode).Where(s => s.Target >= vStart).Where(s => s.Target <= vEnd);
			_context.StatisticsCompleteApplies.RemoveRange(list);
		}

		public void RemoveNewApplies(string companyCode, DateTime vStart, DateTime vEnd)
		{
			var list = _context.StatisticsNewApplies.Where(s => s.CompanyCode == companyCode).Where(s => s.Target >= vStart).Where(s => s.Target <= vEnd);
			_context.StatisticsNewApplies.RemoveRange(list);
		}

		/// <summary>
		/// 进行统计
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="companyCode"></param>
		/// <param name="target"></param>
		/// <param name="db">目标日的申请</param>
		/// <returns>返回是否需要添加到数据库</returns>
		private static Tuple<IEnumerable<T>, bool> GetTargetStatistics<T>(string companyCode, DateTime target, IQueryable<T> db, IQueryable<Apply> applies, IQueryable<RecallOrder> recallDb) where T : IStatisticsApplyBase, new()
		{
			if (db.CheckDb(companyCode, target).Any()) return new Tuple<IEnumerable<T>, bool>(db.CheckDb(companyCode, target), false);
			var records = applies.Select(a => new T()
			{
				From = (byte)(a.BaseInfo.From.SocialInfo.Settle.Self.Address.Code / 10000),
				To = (byte)(a.RequestInfo.VacationPlace.Code / 10000),
				CompanyCode = companyCode,
				Target = target,
				Type = a.BaseInfo.From.CompanyInfo.Duties.Type,
				Day = a.RequestInfo.VacationLength // 此处可能需要仅计算主假
			});
			var groupRecords = records.GroupBy(a => new { a.From, a.To, a.Type });
			var result = groupRecords.ToList().Select(r => new T()
			{
				Target = target,
				To = r.Key.To,
				From = r.Key.From,
				Type = r.Key.Type,
				CompanyCode = companyCode,
				Value = r.Count(),
				Day = r.Sum(a => a.Day)
			});
			return new Tuple<IEnumerable<T>, bool>(result, true);
		}
	}
}