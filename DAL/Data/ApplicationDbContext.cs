﻿using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.Duty;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.Settle;
using DAL.Entities.Vocations;
using DAL.Entities.ZX.Phy;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
		public DbSet<UserApplicationInfo> AppUserApplicationInfos { get; set; }
		public DbSet<UserApplicationSetting> AppUserApplicationSettings { get; set; }
		public DbSet<UserSocialInfo> AppUserSocialInfos { get; set; }
		public DbSet<Settle> Settles { get; set; }
		public DbSet<UserDiyInfo> AppUserDiyInfos { get; set; }
		public DbSet<Avatar> AppUserDiyAvatars { get; set; }
		public DbSet<Company> Companies { get; set; }
		public DbSet<CompanyManagers> CompanyManagers { get; set; }
		public DbSet<AdminDivision> AdminDivisions { get; set; }
		public DbSet<Permissions> Permissions { get; set; }
		public DbSet<Duties> Duties { get; set; }
		public DbSet<DutyType> DutyTypes { get; set; }
		public DbSet<RecallOrder> RecallOrders { get; set; }
		public DbSet<Apply> Applies { get; set; }

		public DbSet<ApplyResponse> ApplyResponses { get; set; }
		public DbSet<ApplyRequest> ApplyRequests { get; set; }
		public DbSet<ApplyBaseInfo> ApplyBaseInfos { get; set; }
		public DbSet<VocationStatistics> VocationStatistics { get; set; }
		public DbSet<VocationStatisticsDescription> VocationStatisticsDescriptions { get; set; }
		/// <summary>
		/// 统计数据值
		/// </summary>
		public DbSet<VocationStatisticsData> VocationStatisticsDatas { get; set; }
		/// <summary>
		/// 统计数据值的具体值
		/// </summary>
		public DbSet<VocationStatisticsDescriptionDataStatusCount>VocationStatisticsDescriptionDataStatusCounts{get;set;}
		public DbSet<VocationDescription> VocationDescriptions { get; set; }
		public DbSet<VocationAdditional> VocationAdditionals { get; set; }

		public DbSet<Subject> Subjects { get; set; }
		public DbSet<Standard> Standards { get; set; }
		public DbSet<UserAction> UserActions { get; set; }
		#endregion

		#region .ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationDbContext"/>.
		/// </summary>
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
			
        }

        #endregion

        #region Logic
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        #endregion
    }
}
