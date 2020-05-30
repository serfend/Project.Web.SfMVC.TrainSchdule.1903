using DAL.DTO.User.Social;
using DAL.Entities;
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
	public static class SettleExtensions
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static SettleDto ToDto(this Settle model)
		{
			if (model == null) return null;
			return new SettleDto()
			{
				Lover = model.Lover.ToDto(),
				LoversParent = model.LoversParent.ToDto(),
				Parent = model.Parent.ToDto(),
				Self = model.Self.ToDto(),
				PrevYearlyComsumeLength = model.PrevYearlyComsumeLength,
			};
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <param name="db"></param>
		/// <returns></returns>
		public static Settle ToModel(this SettleDto model, DbSet<AdminDivision> db)
		{
			if (model == null) return null;
			return new Settle()
			{
				Lover = model.Lover.ToModel(db),
				LoversParent = model.LoversParent.ToModel(db),
				Parent = model.Parent.ToModel(db),
				Self = model.Self.ToModel(db)
			};
		}
	}
}