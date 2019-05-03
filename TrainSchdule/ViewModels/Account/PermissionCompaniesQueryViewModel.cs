using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.BLL.DTO.UserInfo;
using TrainSchdule.DAL.Entities.UserInfo;
using TrainSchdule.DAL.Entities.UserInfo.Permission;
using TrainSchdule.Web.ViewModels;

namespace TrainSchdule.ViewModels.Account
{
	public class PermissionsQueryViewModel:APIDataModel
	{
		public PermissionsQueryDataModel Data { get; set; }
	}

	public class PermissionsQueryDataModel
	{
		public IEnumerable<Permissions> List { get; set; }
	}
}
