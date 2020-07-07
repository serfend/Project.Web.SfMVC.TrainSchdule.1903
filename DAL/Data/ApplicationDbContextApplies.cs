using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.Vacations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Data
{
	public partial class ApplicationDbContext
	{
		public DbSet<RecallOrder> RecallOrders { get; set; }
		public DbSet<ApplyExecuteStatus> ApplyExcuteStatus { get; set; }
		public DbSet<Apply> Applies { get; set; }
		public IQueryable<Apply> AppliesDb { get => Applies.Where(a => !a.IsRemoved); }
		public DbSet<ApplyAuditStream> ApplyAuditStreams { get; set; }
		public DbSet<ApplyAuditStreamSolutionRule> ApplyAuditStreamSolutionRules { get; set; }
		public DbSet<ApplyAuditStreamNodeAction> ApplyAuditStreamNodeActions { get; set; }
		public DbSet<ApplyAuditStep> ApplyAuditSteps { get; set; }
		public DbSet<ApplyResponse> ApplyResponses { get; set; }
		public DbSet<ApplyRequest> ApplyRequests { get; set; }
		public DbSet<ApplyBaseInfo> ApplyBaseInfos { get; set; }
		public DbSet<VacationDescription> VacationDescriptions { get; set; }
		public DbSet<VacationType> VacationTypes { get; set; }

		private void Configuration_Applies(ModelBuilder builder)
		{
			#region VacationTypeData

			builder.Entity<VacationType>().HasData(new VacationType()
			{
				Id = 1,
				Alias = "正休",
				Name = "正休",
				CaculateBenefit = true,
				CanUseOnTrip = true,
				MaxLength = 500,
				Primary = true,
				MinLength = 0,
				MinusNextYear = false,
				NotPermitCrossYear = false,
				RegionOnCompany = ""
			});
			builder.Entity<VacationType>().HasData(new VacationType()
			{
				Id = 2,
				Alias = "事假",
				Name = "事假",
				CaculateBenefit = false,
				CanUseOnTrip = false,
				MaxLength = 30,
				Primary = false,
				MinLength = 0,
				MinusNextYear = true,
				NotPermitCrossYear = false,
				RegionOnCompany = ""
			});

			builder.Entity<VacationType>().HasData(new VacationType()
			{
				Id = 3,
				Alias = "病休",
				Name = "病休",
				CaculateBenefit = false,
				CanUseOnTrip = false,
				MaxLength = 30,
				Primary = false,
				MinLength = 0,
				MinusNextYear = true,
				NotPermitCrossYear = false,
				RegionOnCompany = ""
			});

			builder.Entity<VacationType>().HasData(new VacationType()
			{
				Id = 4,
				Alias = "疫情专项",
				Name = "疫情专项",
				CaculateBenefit = false,
				CanUseOnTrip = false,
				MaxLength = 30,
				Primary = false,
				MinLength = 0,
				MinusNextYear = true,
				NotPermitCrossYear = false,
				RegionOnCompany = ""
			});

			#endregion VacationTypeData
		}
	}
}