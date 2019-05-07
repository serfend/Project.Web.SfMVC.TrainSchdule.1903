using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
		public IEnumerable<CompanyDTO> List { get; set; }
	}
}
