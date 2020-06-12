using BLL.Extensions;
using BLL.Extensions.ApplyExtensions;
using BLL.Interfaces.IVacationStatistics;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.Vacations.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Services.VacationStatistics
{
	/// <summary>
	/// 每日累计统计项
	/// </summary>
	public class StatisticsDailyProcessServices : IStatisticsDailyProcessServices
	{
		private readonly ApplicationDbContext _context;

		public StatisticsDailyProcessServices(ApplicationDbContext context)
		{
			this._context = context;
		}

		public IEnumerable<StatisticsDailyProcessRate> CaculateCompleteApplies(string companyCode, DateTime vStart, DateTime vEnd) => _context.StatisticsDailyProcessRates.AsQueryable()
			.CaculateIStatisticsBaseApplies(_context, GetTargetStatistics, ApplyInfoExtensions.GetCompletedApplies, ApplyInfoExtensions.FirstYearOfTheDay
		, item => _context.StatisticsDailyProcessRates.AddRange(item), companyCode, vStart, vEnd);

		public void RemoveCompleteApplies(string companyCode, DateTime vStart, DateTime vEnd)
		{
			var list = _context.StatisticsDailyProcessRates.Where(s => s.CompanyCode == companyCode).Where(s => s.Target >= vStart).Where(s => s.Target <= vEnd);
			_context.StatisticsDailyProcessRates.RemoveRange(list);
		}

		private Tuple<IEnumerable<T>, bool> GetTargetStatistics<T>(string companyCode, DateTime target, IQueryable<T> db, IQueryable<Apply> applies, IQueryable<RecallOrder> recallDb) where T : StatisticsDailyProcessRate, new()
		{
			if (db.CheckDb(companyCode, target).Any()) return new Tuple<IEnumerable<T>, bool>(db.CheckDb(companyCode, target), false);
			var records = applies.Select(a => new StatisticsAppliesInfo()
			{
				Type = a.BaseInfo.From.CompanyInfo.Duties.Type,
				From = a.BaseInfo.From,
				Days = a.RequestInfo.VacationLength,
				// 此处未考虑召回导致的假期损失
				// 后续可以通过将RecallDb加以考虑
				RecallReduceDay = 0
			});
			var groupRecords = records.GroupBy(a => new { a.Type });
			var companyLength = companyCode.Length;
			var companyAllMembers = _context.AppUsers
				.Where(u => u.Application.Create <= target)
				.Where(u => u.CompanyInfo.Company.Code.Length >= companyLength)
				.Where(u => u.CompanyInfo.Company.Code.Substring(0, companyLength) == companyCode);

			var result = groupRecords.ToList().Select(r =>
			{
				var users = r.GroupBy(a => a.From.Id);
				var companyAtTypeMembers = companyAllMembers.Where(u => u.CompanyInfo.Duties.Type == r.Key.Type);
				var memberCount = companyAtTypeMembers.Count();
				// 每个人的 -> 全年假天，已休天，被召回天
				var userYealyStatisticsDict = new Dictionary<string, YearlyStatistics>();
				foreach (var u in companyAtTypeMembers.ToList())
					userYealyStatisticsDict[u.Id] = new YearlyStatistics()
					{
						YearlyLength = u.SocialInfo.Settle.GetYearlyLengthInner(u, out var m, out var d)
					};
				foreach (var p in users)
				{
					// Sturct 是值类型，不可直接修改
					if (!userYealyStatisticsDict.ContainsKey(p.Key)) userYealyStatisticsDict[p.Key] = new YearlyStatistics();
					var d = userYealyStatisticsDict[p.Key];
					userYealyStatisticsDict[p.Key] = new YearlyStatistics()
					{
						CompleteLength = p.Sum(pInfo => pInfo.Days),
						RecallLength = p.Sum(pInfo => pInfo.RecallReduceDay),
						YearlyLength = d.YearlyLength
					};
				}

				return new T()
				{
					Target = target,
					Type = r.Key.Type,
					CompanyCode = companyCode,
					ApplyMembersCount = users.Count(),
					CompleteYearlyVacationCount = userYealyStatisticsDict.Count(a => a.Value.YearlyLength <= a.Value.CompleteLength - a.Value.RecallLength), // 总假期<=已休-召回
					MembersVacationDayLessThanP60 = userYealyStatisticsDict.Count(a => a.Value.YearlyLength * 0.6 > a.Value.CompleteLength - a.Value.RecallLength), // 总假期>已休-召回
					MembersCount = memberCount,
					CompleteVacationExpectDayCount = userYealyStatisticsDict.Sum(a => a.Value.YearlyLength),
					CompleteVacationRealDayCount = userYealyStatisticsDict.Sum(a => a.Value.CompleteLength - a.Value.RecallLength),
				};
			});
			return new Tuple<IEnumerable<T>, bool>(result, true);
		}

		private struct YearlyStatistics
		{
			/// <summary>
			/// 全年总可用天数
			/// </summary>
			public int YearlyLength { get; set; }

			/// <summary>
			/// 全年完成的天寿
			/// </summary>
			public int CompleteLength { get; set; }

			/// <summary>
			/// 被召回的天数
			/// </summary>
			public int RecallLength { get; set; }
		}
	}
}