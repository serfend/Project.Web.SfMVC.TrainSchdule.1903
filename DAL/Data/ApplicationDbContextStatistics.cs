using DAL.Entities;
using DAL.Entities.Vacations;
using DAL.Entities.Vacations.Statistics;
using DAL.Entities.Vacations.Statistics.StatisticsNewApply;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data
{
	public partial class ApplicationDbContext
	{
		public DbSet<StatisticsApplyNew> StatisticsNewApplies { get; set; }
		public DbSet<StatisticsApplyComplete> StatisticsCompleteApplies { get; set; }
		public DbSet<StatisticsAppliesProcess> StatisticsAppliesProcesses { get; set; }
		public DbSet<StatisticsDailyProcessRate> StatisticsDailyProcessRates { get; set; }

		private void Configuration_Statistics(ModelBuilder builder)
		{
			#region statistics base on time

			builder.Entity<StatisticsAppliesProcess>().HasIndex(s => new { s.Target, s.CompanyCode });
			builder.Entity<StatisticsApplyComplete>().HasIndex(s => new { s.Target, s.CompanyCode });
			builder.Entity<StatisticsApplyNew>().HasIndex(s => new { s.Target, s.CompanyCode });
			builder.Entity<StatisticsDailyProcessRate>().HasIndex(s => new { s.Target, s.CompanyCode });

			#endregion statistics base on time
		}
	}
}