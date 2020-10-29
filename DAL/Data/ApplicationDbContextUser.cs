using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.DiyInfo;
using DAL.Entities.UserInfo.Settle;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Data
{
	public partial class ApplicationDbContext
	{
		public DbSet<User> AppUsers { get; set; }

		/// <summary>
		/// 显示非已被删除的账号
		/// </summary>
		public IQueryable<User> AppUsersDb => AppUsers.Where(u => ((int)u.AccountStatus & (int)AccountStatus.Abolish) == 0);

		public DbSet<UserBaseInfo> AppUserBaseInfos { get; set; }
		public DbSet<UserCompanyInfo> AppUserCompanyInfos { get; set; }
		public DbSet<UserCompanyTitle> UserCompanyTitles { get; set; }
		public DbSet<UserApplicationInfo> AppUserApplicationInfos { get; set; }
		public DbSet<UserApplicationSetting> AppUserApplicationSettings { get; set; }
		public DbSet<UserSocialInfo> AppUserSocialInfos { get; set; }
		public DbSet<Settle> AppUserSocialInfoSettles { get; set; }
		public DbSet<Moment> AppUserSocialInfoSettleMoments { get; set; }
		public DbSet<AppUsersSettleModifyRecord> AppUsersSettleModefyRecord { get; set; }
		public IQueryable<AppUsersSettleModifyRecord> AppUsersSettleModifyRecordDb => AppUsersSettleModefyRecord.ToExistDbSet();

		public DbSet<UserDiyInfo> AppUserDiyInfos { get; set; }
		public DbSet<Avatar> AppUserDiyAvatars { get; set; }
		public DbSet<ThirdpardAccount> ThirdpardAccounts { get; set; }
	}
}