using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.ApplyInfo.DailyApply;
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
		public IQueryable<Apply> AppliesDb => Applies.Where(a => !a.IsRemoved).Where(a => ((int)a.MainStatus & (int)MainStatus.Invalid) == 0);
		public DbSet<ApplyInday> AppliesInday { get; set; }
		public IQueryable<ApplyInday> AppliesIndayDb => AppliesInday.Where(a => !a.IsRemoved).Where(a => ((int)a.MainStatus & (int)MainStatus.Invalid) == 0);
		public DbSet<ApplyAuditStream> ApplyAuditStreams { get; set; }
		public IQueryable<ApplyAuditStream> ApplyAuditStreamsDb => ApplyAuditStreams.Where(a => !a.IsRemoved);
		public DbSet<ApplyAuditStreamSolutionRule> ApplyAuditStreamSolutionRules { get; set; }
		public IQueryable<ApplyAuditStreamSolutionRule> ApplyAuditStreamSolutionRuleDb => ApplyAuditStreamSolutionRules.Where(a => !a.IsRemoved);
		public DbSet<ApplyAuditStreamNodeAction> ApplyAuditStreamNodeActions { get; set; }

		public IQueryable<ApplyAuditStreamNodeAction> ApplyAuditStreamNodeActionDb
			=> ApplyAuditStreamNodeActions.Where(a => !a.IsRemoved);

		public DbSet<ApplyAuditStep> ApplyAuditSteps { get; set; }
		public DbSet<ApplyResponse> ApplyResponses { get; set; }
		public DbSet<ApplyRequest> ApplyRequests { get; set; }
		public DbSet<ApplyIndayRequest> ApplyIndayRequests { get; set; }
		public DbSet<VacationIndayType> VacationIndayTypes { get; set; }
		public DbSet<ApplyBaseInfo> ApplyBaseInfos { get; set; }
		public DbSet<VacationDescription> VacationDescriptions { get; set; }
		public DbSet<VacationType> VacationTypes { get; set; }
		public DbSet<ApplyComment> ApplyComments { get; set; }
		public IQueryable<ApplyComment> ApplyCommentsDb => ApplyComments.Where(c => !c.IsRemoved);
		public DbSet<ApplyCommentLike> ApplyCommentLikes { get; set; }

		private void Configuration_Applies(ModelBuilder builder)
		{
			Configuration_AppliesVacation(builder);
			Configuration_AppliesInday(builder);
		}
		private void Configuration_AppliesVacation(ModelBuilder builder)
        {

			#region VacationTypeData
			var vacaType = builder.Entity<VacationType>();
			vacaType.HasData(new List<VacationType>() {
				new VacationType()
			{
				Id = 1,
				Alias = "正休",
				Name = "正休",
				Description="正常休假",
				CaculateBenefit = true,
				CanUseOnTrip = true,
				MaxLength = 500,
				Primary = true,
				MinLength = 0,
				MinusNextYear = false,
				NotPermitCrossYear = false,
				RegionOnCompany = "",
				Background="vacation_zhengxiu.jpg"
			},
				new VacationType()
			{
				Id = 2,
				Alias = "事假",
				Name = "事假",
				Description="仅可在正休的假期结束后提交，不超过10天。",
				CaculateBenefit = false,
				CanUseOnTrip = false,
				MaxLength = 30,
				Primary = false,
				MinLength = 0,
				MinusNextYear = true,
				NotPermitCrossYear = false,
				RegionOnCompany = "",
				Background="vacation_shijia.jpg"
			},
				new VacationType()
			{
				Id = 3,
				Alias = "病休",
				Name = "病休",
				Description="须提供医院开具的有效证明。",
				CaculateBenefit = false,
				CanUseOnTrip = false,
				MaxLength = 30,
				Primary = false,
				AllowBeforePrimary = true,
				MinLength = 0,
				MinusNextYear = true,
				NotPermitCrossYear = false,
				RegionOnCompany = "",
				Background="vacation_bingxiu.jpg"
			},
				new VacationType()
			{
				Id = 4,
				Alias = "疫情专项",
				Description = "仅限疫情期间14天隔离期使用，将不计算正休假。\n其余情况请使用`确认时间`推迟归队，将从全年假期中扣除期间延迟归队的假期天数。",
				Name = "疫情专项",
				CaculateBenefit = false,
				CanUseOnTrip = false,
				MaxLength = 30,
				Primary = false,
				AllowBeforePrimary = true,
				MinLength = 0,
				MinusNextYear = true,
				NotPermitCrossYear = false,
				RegionOnCompany = "",
				Background="vacation_yiqingzhuanxiang.jpg"
			}
			});
			#endregion VacationTypeData
		}
		private void Configuration_AppliesInday(ModelBuilder builder)
		{
			#region VacationTypeData

			var vacaType = builder.Entity<VacationIndayType>();
			vacaType.HasData(new List<VacationIndayType>() {
				new VacationIndayType()
			{
				Id = 1,
				Alias = "外出",
				Name = "外出",
				Description="办事、购物、休闲、一日内看病等利用非工作日的因私外出活动",
				PermitCrossDay=0,
				RegionOnCompany = "",
				Background="inday_outdoor.jpg"
			},new VacationIndayType()
			{
				Id = 2,
				Alias = "出差",
				Name = "出差",
				Description="公差、接车/机、开会、保障等非集体活动外出",
				PermitCrossDay=7,
				RegionOnCompany = "",
				Background="inday_business.jpg"
			},new VacationIndayType()
			{
				Id = 3,
				Alias = "回家",
				Name = "回家",
				Description="工作日下班后、节假日全天等跨日外出",
				PermitCrossDay=3,
				RegionOnCompany = "",
				Background="inday_family.jpg"
			},
			});
			#endregion VacationTypeData
		}
	}
}