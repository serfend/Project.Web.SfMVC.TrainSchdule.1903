using BLL.Interfaces;
using DAL.DTO.User;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace BLL.Extensions
{
	public static class UserExtensions
	{
		public const string InviteByInvalidValue = "00Invalid";

		public enum AccountType
		{
			Deny = -1,
			NotBeenAuth = 0,
			BeenAuth = 1
		}

		public static AccountType InvalidAccount(this UserApplicationInfo app)
		{
			if (app == null) return AccountType.NotBeenAuth;
			var inviteBy = app.InvitedBy;
			if (inviteBy == null) return AccountType.NotBeenAuth;
			if (inviteBy == InviteByInvalidValue) return AccountType.Deny;
			return AccountType.BeenAuth;
		}

		public static UserSummaryDto ToSummaryDto(this User user)
		{
			if (user == null) return null;
			var diyInfo = user.DiyInfo ?? new UserDiyInfo() { Avatar = new Avatar() };
			var companyInfo = user.CompanyInfo ?? new UserCompanyInfo() { Company = new DAL.Entities.Company(),Duties = new DAL.Entities.Duties(),Title=new UserCompanyTitle()};
			var baseInfo = user.BaseInfo ?? new UserBaseInfo() { Gender=GenderEnum.Unknown };
			return new UserSummaryDto()
			{
				About = diyInfo.About ?? "无简介",
				Avatar = diyInfo.Avatar?.Id.ToString(),
				CompanyCode = companyInfo.Company?.Code,
				DutiesCode = companyInfo.Duties?.Code,
				CompanyName = companyInfo.Company?.Name ?? "无单位",
				DutiesName = companyInfo.Duties?.Name ?? "无职务",
				UserTitle = companyInfo.Title?.Name ?? "无等级",
				UserTitleDate = companyInfo.TitleDate,
				Gender = baseInfo.Gender,
				RealName = baseInfo.RealName ?? "无姓名",
				TimeBirth = baseInfo.Time_BirthDay,
				TimeWork = baseInfo.Time_Work,
				Hometown = baseInfo.Hometown,
				Id = user.Id,
				IsInitPassword = baseInfo.PasswordModify ,
				InviteBy = user.Application?.InvitedBy
			};
		}

		/// <summary>
		/// 按单位-职务等级-职级等级的顺序依次排序
		/// </summary>
		/// <param name="users"></param>
		/// <returns></returns>
		public static IOrderedQueryable<User> OrderByCompanyAndTitle(this IQueryable<User> users) => users.OrderBy(u => u.CompanyInfo.Company.Code).OrderByLevel();

		/// <summary>
		/// 按单位-职务等级-职级等级的顺序依次排序
		/// </summary>
		/// <param name="users"></param>
		/// <returns></returns>
		public static IOrderedQueryable<User> OrderByCompanyAndTitle(this IOrderedQueryable<User> users) => users.ThenBy(u => u.CompanyInfo.Company.Code).OrderByLevel();

		public static IOrderedQueryable<User> OrderByLevel(this IOrderedQueryable<User> users) => users.ThenByDescending(u => u.CompanyInfo.Duties.Level).ThenByDescending(u => u.CompanyInfo.Title.Level);
	}
}