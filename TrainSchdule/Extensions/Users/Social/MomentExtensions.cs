using DAL.DTO.User.Social;
using DAL.Entities;
using DAL.Entities.UserInfo.Settle;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TrainSchdule.Extensions.Users.Social
{
	/// <summary>
	///
	/// </summary>
	public static class MomentExtensions
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <param name="db"></param>
		/// <param name="raw"></param>
		/// <returns></returns>
		public static Moment ToModel(this MomentDto model, DbSet<AdminDivision> db, Moment raw = null)
		{
			if (model == null) return raw;
			if (raw == null) raw = new Moment();
			var address = model?.Address?.Code;
			raw.Address = db.FirstOrDefault(a => a.Code == address);
			raw.AddressDetail = model.AddressDetail;
			raw.Date = model.Date;
			raw.Valid = model.Valid;
			//if (tmp.Address == null || tmp.Date.Year < 1900 || tmp.AddressDetail == "") return null;
			return raw;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static MomentDto ToDto(this Moment model)
		{
			if (model == null) return null;
			return new MomentDto()
			{
				Address = model.Address,
				AddressDetail = model.AddressDetail,
				Date = model.Date,
				Valid = model.Valid
			};
		}
	}
}