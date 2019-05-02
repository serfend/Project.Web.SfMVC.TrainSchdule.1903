using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.BLL.DTO;
using TrainSchdule.DAL.Entities.UserInfo;
using TrainSchdule.Web.ViewModels;

namespace TrainSchdule.ViewModels.Account
{
	public class PermissionCompaniesQueryViewModel:APIDataModel
	{
		public PermissionCompaniesQueryDataModel Data { get; set; }
	}

	public class PermissionCompaniesQueryDataModel
	{
		public IEnumerable<PermissionCompanyDTO> List { get; set; }
	}
}
