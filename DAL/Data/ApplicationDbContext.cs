using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.BBS;
using DAL.Entities.Common;
using DAL.Entities.FileEngine;
using DAL.Entities.Game_r3;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.Settle;
using DAL.Entities.Vacations;
using DAL.Entities.ZX.Phy;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using User = DAL.Entities.UserInfo.User;

namespace DAL.Data
{
	/// <summary>
	/// Main DB context in the application.
	/// </summary>
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		#region Properties

		public DbSet<User> AppUsers { get; set; }
		public DbSet<UserBaseInfo> AppUserBaseInfos { get; set; }
		public DbSet<UserCompanyInfo> AppUserCompanyInfos { get; set; }
		public DbSet<UserCompanyTitle> UserCompanyTitles { get; set; }
		public DbSet<UserApplicationInfo> AppUserApplicationInfos { get; set; }
		public DbSet<UserApplicationSetting> AppUserApplicationSettings { get; set; }
		public DbSet<UserSocialInfo> AppUserSocialInfos { get; set; }
		public DbSet<Settle> AUserSocialInfoSettles { get; set; }
		public DbSet<Moment> AppUserSocialInfoSettleMoments { get; set; }
		public DbSet<UserDiyInfo> AppUserDiyInfos { get; set; }
		public DbSet<Avatar> AppUserDiyAvatars { get; set; }
		public DbSet<Company> Companies { get; set; }
		public DbSet<CompanyManagers> CompanyManagers { get; set; }
		public DbSet<AdminDivision> AdminDivisions { get; set; }
		public DbSet<Permissions> Permissions { get; set; }
		public DbSet<Duties> Duties { get; set; }
		public DbSet<RecallOrder> RecallOrders { get; set; }
		public DbSet<Apply> Applies { get; set; }
		public IQueryable<Apply> AppliesDb { get => Applies.Where(a => !a.IsRemoved); }
		public DbSet<ApplyAuditStream> ApplyAuditStreams { get; set; }
		public DbSet<ApplyAuditStreamSolutionRule> ApplyAuditStreamSolutionRules { get; set; }
		public DbSet<ApplyAuditStreamNodeAction> ApplyAuditStreamNodeActions { get; set; }
		public DbSet<ApplyAuditStep> ApplyAuditSteps { get; set; }
		public DbSet<ApplyResponse> ApplyResponses { get; set; }
		public DbSet<ApplyRequest> ApplyRequests { get; set; }
		public DbSet<ApplyBaseInfo> ApplyBaseInfos { get; set; }
		public DbSet<XlsTemplete> XlsTempletes { get; set; }
		public DbSet<VacationStatistics> VacationStatistics { get; set; }
		public DbSet<VacationStatisticsDescription> VacationStatisticsDescriptions { get; set; }

		/// <summary>
		/// 统计数据值
		/// </summary>
		public DbSet<VacationStatisticsData> VacationStatisticsDatas { get; set; }

		public DbSet<VacationDescription> VacationDescriptions { get; set; }
		public DbSet<VacationAdditional> VacationAdditionals { get; set; }

		public DbSet<Subject> Subjects { get; set; }
		public DbSet<Standard> Standards { get; set; }

		/// <summary>
		/// 近90天的数据
		/// </summary>
		public IQueryable<UserAction> UserActionsDb { get => UserActions.Where(u => u.Date > DateTime.Today.AddDays(-90)); }

		public DbSet<UserAction> UserActions { get; set; }

		public DbSet<Post> Posts { get; set; }
		public DbSet<DAL.Entities.BBS.PostContent> PostContents { get; set; }
		public IQueryable<DAL.Entities.BBS.PostContent> PostContentsDb { get => PostContents.Where(p => !p.IsRemoved); }
		public DbSet<DAL.Entities.BBS.Like> PostLikes { get; set; }

		public DbSet<GiftCode> GiftCodes { get; set; }
		public DbSet<DAL.Entities.Game_r3.User> GameR3Users { get; set; }
		public DbSet<DAL.Entities.Game_r3.UserInfo> GameR3UserInfos { get; set; }
		public DbSet<GainGiftCode> GainGiftCodeHistory { get; set; }
		public DbSet<SignIn> SignIns { get; set; }
		public DbSet<UserFile> UserFiles { get; set; }
		public DbSet<UserFileInfo> UserFileInfos { get; set; }
		public IQueryable<UserFileInfo> UserFileInfosDb => UserFileInfos.Where(c => !c.IsRemoved);
		public DbSet<FileUploadStatus> FileUploadStatuses { get; set; }
		public DbSet<UploadCache> UploadCaches { get; set; }
		public DbSet<ShortUrl> CommonShortUrl { get; set; }
		public IQueryable<ShortUrl> CommonShortUrlDb => CommonShortUrl.Where(c => !c.IsRemoved).Where(c => c.Expire > DateTime.Now);
		public DbSet<ShortUrlStatistics> CommonShortUrlStatistics { get; set; }

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
		}

		#endregion Logic
	}
}