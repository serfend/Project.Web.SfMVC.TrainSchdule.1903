using System.Collections.Generic;
using DAL.DTO.Company;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.User
{
	public class UserAuditStreamViewModel:APIDataModel
	{
		public UserAuditStreamDataModel Data { get; set; }
	}

	public class UserAuditStreamDataModel
	{
		public IEnumerable<CompanyDto> List { get; set; }
	}

}
