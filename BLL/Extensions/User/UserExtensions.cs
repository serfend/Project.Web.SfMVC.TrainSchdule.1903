using BLL.Interfaces;
using DAL.DTO.User;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Hosting;
using Remotion.Linq.Parsing.ExpressionVisitors.MemberBindings;
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
			if (inviteBy == InviteByInvalidValue || inviteBy == "invalid") return AccountType.Deny;
			return AccountType.BeenAuth;
		}

		public static UserSummaryDto ToSummaryDto(this User user)
		{
			if (user == null) return null;
			var inviteBy = user.Application?.InvitedBy;
			inviteBy = inviteBy == "invalid" ? InviteByInvalidValue : inviteBy;
			var b = new UserSummaryDto()
			{
				About = user.DiyInfo?.About ?? "无简介",
				Avatar = user.DiyInfo?.Avatar?.Id.ToString(),
				CompanyCode = user.CompanyInfo?.Company?.Code,
				DutiesCode = user.CompanyInfo?.Duties?.Code,
				CompanyName = user.CompanyInfo?.Company?.Name ?? "无单位",
				DutiesName = user.CompanyInfo?.Duties?.Name ?? "无职务",
				UserTitle = user.CompanyInfo?.Title?.Name ?? "无等级",
				UserTitleDate = user.CompanyInfo?.TitleDate,
				Gender = user.BaseInfo.Gender,
				RealName = user.BaseInfo?.RealName ?? "无姓名",
				TimeBirth = user.BaseInfo?.Time_BirthDay,
				TimeWork = user.BaseInfo?.Time_Work,
				Hometown = user.BaseInfo?.Hometown,
				Id = user.Id,
				IsInitPassword = user.BaseInfo.PasswordModefy,
				InviteBy = inviteBy
			};
			return b;
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