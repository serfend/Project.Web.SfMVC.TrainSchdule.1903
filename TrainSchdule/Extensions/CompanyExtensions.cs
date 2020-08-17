using DAL.Entities;
using DAL.Entities.UserInfo;
using System;
using TrainSchdule.ViewModels.Company;

namespace TrainSchdule.Extensions
{
	/// <summary>
	///
	/// </summary>
	public static class CompanyExtensions
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static CompanyChildDataModel ToCompanyModel(this Company model)
		{
			var b = new CompanyChildDataModel()
			{
				Code = model.Code,
				Name = model.Name
			};
			return b;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static DutyDataModel ToDataModel(this Duties model)
		{
			if (model == null) return null;
			return new DutyDataModel()
			{
				Code = model.Code,
				IsMajorManager = model.IsMajorManager,
				Name = model.Name,
				Tags = model.Tags?.Length == 0 ? Array.Empty<string>() : model.Tags?.Split("##")
			};
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static UserTitleDataModel ToDataModel(this UserCompanyTitle model)
		{
			if (model == null) return null;
			return new UserTitleDataModel()
			{
				Code = model.Code,
				Level = model.Level,
				Name = model.Name
			};
		}
	}
}