using System;
using DAL.DTO.User;
using DAL.Entities;
using DAL.Entities.UserInfo;
using Microsoft.EntityFrameworkCore;
using TrainSchdule.ViewModels.Account;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.User;
using TrainSchdule.Extensions.Users.Social;
using DAL.Entities.UserInfo.DiyInfo;

namespace TrainSchdule.Extensions
{
	/// <summary>
	///
	/// </summary>
	public static class UserExtensions
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <param name="invitedBy"></param>
		/// <param name="dbAdmin"></param>
		/// <param name="dbThidpardAccount"></param>
		/// <returns></returns>
		public static User ToModel(this UserModefyDataModel model, string invitedBy, DbSet<AdminDivision> dbAdmin, DbSet<ThirdpardAccount> dbThidpardAccount)
		{
			if (model == null) return null;
			var u = new User()
			{
				Id = model.Application.UserName,
				CompanyInfo = new UserCompanyInfo()
				{
					Company = new Company()
					{
						Code = model.Company?.Company?.Code
					},
					Duties = new Duties()
					{
						Name = model.Company?.Duties?.Name
					},
					Title = new UserCompanyTitle()
					{
						Name = model.Company?.Title?.Name
					},
					TitleDate = model.Company?.TitleDate
				},
				Application = model.Application?.ToModel(invitedBy),
				BaseInfo = model.Base,
				SocialInfo = model.Social?.ToModel(dbAdmin),
				DiyInfo = model.Diy?.ToModel(dbThidpardAccount)
			};
			if (u.BaseInfo != null) u.BaseInfo.Id = Guid.Empty;
			return u;
		}
	}
}