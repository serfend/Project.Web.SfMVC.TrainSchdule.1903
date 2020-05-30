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
		/// <returns></returns>
		public static Moment ToModel(this MomentDto model, DbSet<AdminDivision> db)
		{
			var tmp = new Moment()
			{
				Address = db.Where(a => a.Code == model.Address.Code).FirstOrDefault(),
				AddressDetail = model.AddressDetail,
				Date = model.Date,
				Valid = model.Valid
			};
			//if (tmp.Address == null || tmp.Date.Year < 1900 || tmp.AddressDetail == "") return null;
			return tmp;
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