using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.BLL.DTO;
using TrainSchdule.DAL.Entities;
using TrainSchdule.Web.ViewModels;

namespace TrainSchdule.ViewModels.Account
{
	public class PermissionCompaniesQueryViewModel:APIViewModel
	{
		public IEnumerable<PermissionCompanyDTO> Data { get; set; }
	}
}
