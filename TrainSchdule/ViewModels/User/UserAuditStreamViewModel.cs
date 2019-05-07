using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.DTO.Company;
using TrainSchdule.Web.ViewModels;

namespace TrainSchdule.ViewModels.User
{
	public class UserAuditStreamViewModel:APIDataModel
	{
		public UserAuditStreamDataModel Data { get; set; }
	}

	public class UserAuditStreamDataModel
	{
		public IEnumerable<CompanyDTO> List { get; set; }
	}

}
