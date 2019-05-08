using System.Collections.Generic;
using DAL.DTO.User;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Company
{
	public class CompanyManagerViewModel:APIDataModel
	{
		public CompanyManagerDataModel Data { get; set; }
	}

	public class CompanyManagerDataModel
	{
		public IEnumerable<UserSummaryDto> List { get; set; }
	}
}
