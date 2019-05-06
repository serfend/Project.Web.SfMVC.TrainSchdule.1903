using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Entities.UserInfo;
using TrainSchdule.Web.ViewModels;

namespace TrainSchdule.ViewModels.User
{
	public class UserCompanyInfoViewModel:APIDataModel
	{
		public UserCompanyInfoDataModel Data { get; set; }
	}

	public class UserCompanyDataModel
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public string Parent { get; set; }
	}
	public class UserCompanyInfoDataModel
	{
		public UserCompanyDataModel Company { get; set; }
		public string Duties { get; set; }
	}

	public class UserDutiesViewModel : APIDataModel
	{
		public UserDutiesDataModel Data { get; set; }
	}

	public class UserDutiesDataModel
	{
		public int? Code { get; set; }
		public string Name { get; set; }
	}
	public static class UserCompanyInfoExtensions
	{
		public static UserCompanyInfoDataModel ToCompanyModel(this UserCompanyInfo model,ICompaniesService companiesService)
		{
			return new UserCompanyInfoDataModel()
			{
				Company = new UserCompanyDataModel()
				{
					Code = model.Company?.Code,
					Name = model.Company?.Name,
					Parent = companiesService.FindParent(model.Company?.Code)?.Name
				},
				Duties = model.Duties?.Name
			};
		}

		public static UserDutiesDataModel ToDutiesModel(this UserCompanyInfo model)
		{
			return new UserDutiesDataModel()
			{
				Code = model.Duties?.Code,
				Name = model.Duties?.Name
			};
		}
	}
}
