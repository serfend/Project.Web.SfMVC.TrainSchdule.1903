using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using TrainSchdule.ViewModels.Company;

namespace TrainSchdule.Extensions
{
	public static class CompanyExtensions
	{
		public static CompanyChildDataModel ToCompanyModel(this Company model)
		{
			var b=new CompanyChildDataModel()
			{
				Code = model.Code,
				Name = model.Name
			};
			return b;
		}
	}
}
