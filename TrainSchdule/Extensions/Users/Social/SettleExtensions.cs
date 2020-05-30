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
		/// <param name="raw"></param>
		/// <returns></returns>
		public static Settle ToModel(this SettleDto model, DbSet<AdminDivision> db, Settle raw = null)
		{
			if (model == null) return raw;
			if (raw == null) raw = new Settle();

			raw.Lover = model.Lover.ToModel(db, raw.Lover);
			raw.LoversParent = model.LoversParent.ToModel(db, raw.LoversParent);
			raw.Parent = model.Parent.ToModel(db, raw.Parent);
			raw.Self = model.Self.ToModel(db, raw.Self);
			return raw;
		}
	}
}