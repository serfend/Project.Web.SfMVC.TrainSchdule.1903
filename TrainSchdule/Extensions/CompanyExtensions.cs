using DAL.Entities;
using DAL.Entities.Duty;
using DAL.Entities.UserInfo;
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
			var b=new CompanyChildDataModel()
			{
				Code = model.Code,
				Name = model.Name
			};
			return b;
		}
		public static DutyDataModel ToDataModel(this Duties model)
		{
			if (model == null) return null;
			return new DutyDataModel()
			{
				DutiesType = null,//用于转换后手动
				Code = model.Code,
				IsMajorManager = model.IsMajorManager,
				Name = model.Name,
				DutyType = model.DutiesRawType
			};
		}
		public static UserTitleDataModel ToDataModel(this UserCompanyTitle model)
		{
			if (model == null) return null;
			return new UserTitleDataModel()
			{
				Code=model.Code,
				Level=model.Level,
				Name=model.Name
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static DutyTypeDataModel ToDataModel(this DutyType type)
		{
			if (type == null) return null;
			return new DutyTypeDataModel()
			{
				AuditLevelNum = type.AuditLevelNum,
				Code = type.Code,
				Name = type.Name
			};
		}
	}
}
