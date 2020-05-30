using DAL.DTO.User.Social;
using DAL.Entities;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.Settle;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.Extensions.Users.Social
{
	/// <summary>
	///
	/// </summary>
	public static class SocialExtensions
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static SocialDto ToDto(this UserSocialInfo model)
		{
			if (model == null) return null;
			return new SocialDto()
			{
				Address = model.Address,
				AddressDetail = model.AddressDetail,
				Phone = model.Phone,
				Settle = model.Settle.ToDto()
			};
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <param name="db"></param>
		/// <returns></returns>
		public static UserSocialInfo ToModel(this SocialDto model, DbSet<AdminDivision> db)
		{
			if (model == null) return null;
			return new UserSocialInfo()
			{
				Address = model.Address,
				AddressDetail = model.AddressDetail,
				Phone = model.Phone,
				Settle = model.Settle.ToModel(db)
			};
		}
	}
}