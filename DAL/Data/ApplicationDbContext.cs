using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.BBS;
using DAL.Entities.Common;
using DAL.Entities.FileEngine;
using DAL.Entities.Game_r3;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.Settle;
using DAL.Entities.Vacations;
using DAL.Entities.Vacations.Statistics;
using DAL.Entities.Vacations.Statistics.StatisticsNewApply;
using DAL.Entities.ZX.Phy;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DAL.Data
{
	/// <summary>
	/// Main DB context in the application.
	/// </summary>
	public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		#region Properties

		public DbSet<Company> Companies { get; set; }
		public DbSet<CompanyManagers> CompanyManagers { get; set; }
		public DbSet<AdminDivision> AdminDivisions { get; set; }
		public DbSet<Permissions> Permissions { get; set; }
		public DbSet<Duties> Duties { get; set; }

		public DbSet<XlsTemplete> XlsTempletes { get; set; }

		public DbSet<VacationAdditional> VacationAdditionals { get; set; }

		public DbSet<GradePhySubject> GradePhySubjects { get; set; }
		public DbSet<GradePhyStandard> GradePhyStandards { get; set; }

		/// <summary>
		/// 近90天的数据
		/// </summary>
		public IQueryable<UserAction> UserActionsDb => UserActions.Where(u => u.Date > DateTime.Today.AddDays(-90));

		public DbSet<UserAction> UserActions { get; set; }

		public DbSet<Post> Posts { get; set; }
		public DbSet<DAL.Entities.BBS.PostContent> PostContents { get; set; }
		public IQueryable<DAL.Entities.BBS.PostContent> PostContentsDb => PostContents.ToExistDbSet();
		public DbSet<DAL.Entities.BBS.Like> PostLikes { get; set; }

		public DbSet<GiftCode> GiftCodes { get; set; }
		public DbSet<DAL.Entities.Game_r3.User> GameR3Users { get; set; }
		public DbSet<DAL.Entities.Game_r3.UserInfo> GameR3UserInfos { get; set; }
		public DbSet<GainGiftCode> GainGiftCodeHistory { get; set; }
		public DbSet<SignIn> SignIns { get; set; }

		public DbSet<ApplicationUpdateRecord> ApplicationUpdateRecords { get; set; }
		public IQueryable<ApplicationUpdateRecord> ApplicationUpdateRecordsDb => ApplicationUpdateRecords.Where(r => !r.IsRemoved);

		#endregion Properties

		#region .ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationDbContext"/>.
		/// </summary>
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		#endregion .ctors

		#region Logic

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<ShortUrl>().HasIndex(s => s.Key);
			builder.Entity<UserAction>().HasIndex(s => s.Date);
			builder.Entity<ApplicationUpdateRecord>().HasIndex(s => s.Create);

			#region statistics base on time

			builder.Entity<StatisticsAppliesProcess>().HasIndex(s => new { s.Target, s.CompanyCode });
			builder.Entity<StatisticsApplyComplete>().HasIndex(s => new { s.Target, s.CompanyCode });
			builder.Entity<StatisticsApplyNew>().HasIndex(s => new { s.Target, s.CompanyCode });
			builder.Entity<StatisticsDailyProcessRate>().HasIndex(s => new { s.Target, s.CompanyCode });

			#endregion statistics base on time
		}

		#endregion Logic
	}
}