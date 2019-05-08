using System.Collections.Generic;
using DAL.DTO.Company;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.User
{
	public class UserManageRangeViewModel:APIDataModel
	{
		public UserManageRangeDataModel Data { get; set; }
	}

	public class UserManageRangeDataModel
	{
		public IEnumerable<CompanyDto> List { get; set; }
	}
}
