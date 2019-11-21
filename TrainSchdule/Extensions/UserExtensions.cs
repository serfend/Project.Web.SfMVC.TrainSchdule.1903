using System;
using DAL.DTO.User;
using DAL.Entities;
using DAL.Entities.UserInfo;
using Microsoft.EntityFrameworkCore;
using TrainSchdule.ViewModels.Account;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.User;

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
		/// <param name="db"></param>
		/// <returns></returns>
		public static User ToDTO(this UserCreateDataModel model, string invitedBy, DbSet<AdminDivision> db)
		{
			if (model == null) return null;
			var settle = model.Social.Settle;
			return new User()
			{
				Id=model.Application.UserName,
				CompanyInfo = new UserCompanyInfo()
				{
					Company = new Company()
					{
						Code = model.Company.Company.Code
					},
					Duties = new Duties()
					{
						Name = model.Company.Duties.Name
					}
				},
				Application = model.Application.ToModel(),
				BaseInfo =model.Base,
				SocialInfo = model.Social.ToModel(),
				TrainInfo = new UserTrainInfo(),//TODO 后期可能需要加上受训情况
				DiyInfo=model.Diy.ToModel(new UserDiyInfo())
			};
		}
	}
}
