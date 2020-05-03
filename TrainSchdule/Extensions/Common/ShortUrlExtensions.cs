using DAL.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Common;

namespace TrainSchdule.Extensions.Common
{
	/// <summary>
	///
	/// </summary>
	public static class ShortUrlExtensions
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static ShortUrlCreateDataModel ToDataModel(this ShortUrl model)
		{
			return new ShortUrlCreateDataModel()
			{
				CreateBy = model.CreateBy?.Id,
				Create = model.Create,
				Expire = model.Expire,
				Key = model.Key,
				Target = model.Target
			};
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static ShortUrlStatisticsSingleDataModel ToDataModel(this ShortUrlStatistics model)
		{
			return new ShortUrlStatisticsSingleDataModel()
			{
				Create = model.Create,
				Device = model.Device,
				Ip = model.Ip,
				UA = model.UA,
				ViewBy = model.ViewBy.Id
			};
		}
	}
}