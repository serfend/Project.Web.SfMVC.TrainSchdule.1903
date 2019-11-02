using DAL.Entities;
using DAL.Entities.UserInfo.Settle;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.User;

namespace TrainSchdule.Extensions
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
		public static Moment ToMoment(this MomentDataModel model, DbSet<AdminDivision> db)
		{
			var tmp= new Moment()
			{
				Address = db.Where<AdminDivision>(a => a.Code == model.Address).FirstOrDefault(),
				AddressDetail = model.AddressDetail,
				Date = model.Date,
				Valid = model.Valid
			};
			if (tmp.Address == null || tmp.Date.Year < 1900||tmp.AddressDetail=="") return null;
			return tmp;
		}
	}
}
