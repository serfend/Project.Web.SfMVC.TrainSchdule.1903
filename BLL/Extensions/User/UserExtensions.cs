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
		public static string GetCompanyMajor(this UserCompanyInfo uc)
		{
			var ud = uc.Duties.IsMajorManager;
			if (!ud) return null;
			return uc.CompanyCode;
		}
		public static bool CheckCompanyManager(this User user,string targetCompany, IUserServiceDetail userServiceDetail)
        {
			var results = userServiceDetail.InMyManage(user).Result;
			if (targetCompany == null && results.Item2 > 0) return true; // 如果无授权对象，则有任意单位权限即可
			else if (results.Item2 > 0 && results.Item1.Any(c => targetCompany.Length >= c.Code.Length && targetCompany.StartsWith(c.Code)))
				return true;
			return false;
		}
		public static bool CheckCompanyMajor(this User user,string targetCompany)
        {
			var companyMajor = user.CompanyInfo.GetCompanyMajor();
			return targetCompany == null || (companyMajor != null && targetCompany.Length >= companyMajor.Length && targetCompany.StartsWith(companyMajor));
		}
		public static UserSummaryDto ToSummaryDto(this User user)
		{
			if (user == null) return null;
			var diyInfo = user.DiyInfo ?? new UserDiyInfo() { Avatar = new Avatar() };
			var companyInfo = user.CompanyInfo ?? new UserCompanyInfo() { Company = new DAL.Entities.Company(),Duties = new DAL.Entities.Duties(),Title=new UserCompanyTitle()};
			var baseInfo = user.BaseInfo ?? new UserBaseInfo() { Gender=GenderEnum.Unknown };
			var company = companyInfo.Company;
			var duties = companyInfo.Duties;
			return new UserSummaryDto()
			{
				About = diyInfo.About ?? "无简介",
				Avatar = diyInfo.Avatar?.Id.ToString(),
				CompanyCode = companyInfo.CompanyCode,
				Cid=baseInfo.Cid,
				DutiesCode = duties?.Code,
				CompanyName = company?.Name ?? "无单位",
				DutiesName = duties?.Name ?? "无职务",
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
		/// 按单位-资历的顺序依次排序
		/// </summary>
		/// <param name="users"></param>
		/// <returns></returns>
		public static IOrderedQueryable<User> OrderByCompanyAndTitle(this IQueryable<User> users) => users.OrderBy(u => u.CompanyInfo.CompanyCode).OrderByLevel();

		/// <summary>
		/// 按单位-资历的顺序依次排序
		/// </summary>
		/// <param name="users"></param>
		/// <returns></returns>
		public static IOrderedQueryable<User> OrderByCompanyAndTitle(this IOrderedQueryable<User> users) => users.ThenBy(u => u.CompanyInfo.CompanyCode).OrderByLevel();
		/// <summary>
		/// 按职务等级-职级等级-工作时间依次排序
		/// </summary>
		/// <param name="users"></param>
		/// <returns></returns>
		public static IOrderedQueryable<User> OrderByLevel(this IOrderedQueryable<User> users) => users.ThenByDescending(u => u.CompanyInfo.Duties.Level).ThenByDescending(u => u.CompanyInfo.Title.Level).ThenBy(u=>u.BaseInfo.Time_Work);
	}
}