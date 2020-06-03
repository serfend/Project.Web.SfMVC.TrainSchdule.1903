using DAL.Entities;
using DAL.Entities.Vacations.Statistics.StatisticsNewApply;
using DAL.Entities.Vacations.VacationsStatistics;
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
		public DbSet<VacationStatistics> VacationStatistics { get; set; }
		public DbSet<VacationStatisticsDescription> VacationStatisticsDescriptions { get; set; }

		/// <summary>
		/// 统计数据值
		/// </summary>
		public DbSet<VacationStatisticsDescriptionData> VacationStatisticsDatas { get; set; }

		public DbSet<VacationDescription> VacationDescriptions { get; set; }
	}
}