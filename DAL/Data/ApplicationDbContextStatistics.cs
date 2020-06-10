using DAL.Entities;
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
	}
}