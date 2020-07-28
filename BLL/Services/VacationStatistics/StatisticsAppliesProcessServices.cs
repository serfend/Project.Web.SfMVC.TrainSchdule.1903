using BLL.Extensions;
using BLL.Extensions.ApplyExtensions;
using BLL.Extensions.Common;
using BLL.Interfaces.IVacationStatistics;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using DAL.Entities.Vacations;
using DAL.Entities.Vacations.Statistics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Services.VacationStatistics
{
	/// <summary>
	/// 统计休假天数和次数
	/// </summary>
	public class StatisticsAppliesProcessServices : IStatisticsAppliesProcessServices
	{
		private readonly ApplicationDbContext _context;

		public StatisticsAppliesProcessServices(ApplicationDbContext context)
		{
			this._context = context;
		}

		public IEnumerable<StatisticsAppliesProcess> CaculateCompleteApplies(string companyCode, DateTime vStart, DateTime vEnd) => _context.StatisticsAppliesProcesses.AsQueryable()
			.CaculateIStatisticsBaseApplies(_context, GetTargetStatistics, ApplyInfoExtensions.GetCompletedApplies, ApplyInfoExtensions.LastMilliSecondInDay
				, item => _context.StatisticsAppliesProcesses.AddRange(item), companyCode, vStart, vEnd);

		public IEnumerable<StatisticsAppliesProcess> DirectGetCompleteApplies(string companyCode, DateTime vStart, DateTime vEnd) => _context.StatisticsAppliesProcesses.AsQueryable().CheckDb(companyCode, vStart, vEnd);

		public void RemoveCompleteApplies(string companyCode, DateTime vStart, DateTime vEnd)
		{
			var pattern = $"{companyCode}%";
			var list = _context.StatisticsAppliesProcesses.Where(s => EF.Functions.Like(s.CompanyCode, pattern)).Where(s => s.Target >= vStart).Where(s => s.Target <= vEnd);
			_context.StatisticsAppliesProcesses.RemoveRange(list);
		}

		public static Tuple<IEnumerable<T>, bool> GetTargetStatistics<T>(string companyCode, DateTime target, IQueryable<T> db, IQueryable<Apply> applies, IQueryable<RecallOrder> recallDb) where T : StatisticsAppliesProcess, new()
		{
			if (db.CheckDb(companyCode, target).Any()) return new Tuple<IEnumerable<T>, bool>(db.CheckDb(companyCode, target), false);
			var records = applies.Select(a => new StatisticsAppliesInfo()
			{
				Type = a.BaseInfo.From.CompanyInfo.Duties.Type,
				From = a.BaseInfo.From,
				Days = a.RequestInfo.VacationLength,
				// 统计中用不到RecallOrder
				RecallReduceDay = 0
			}).ToList(); // TODO client side query shoud disabled
			var groupRecords = records.GroupBy(a => new { a.Type }).ToList();
			var result = groupRecords.Select(r =>
			{
				var users = r.GroupBy(a => a.From.Id);
				var dict = new Dictionary<string, int>();
				foreach (var p in users)
				{
					var pCompleteLength = p.Sum(pRecord => pRecord.Days);
					dict[p.Key] = pCompleteLength;
				}
				return new T()
				{
					Target = target,
					Type = r.Key.Type,
					CompanyCode = companyCode,
					ApplyCount = r.Count(),
					ApplySumDayCount = dict.Sum(a => a.Value)
				};
			});
			return new Tuple<IEnumerable<T>, bool>(result, true);
		}
	}
}