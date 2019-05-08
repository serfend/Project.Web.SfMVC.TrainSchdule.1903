using System.Collections.Generic;
using DAL.Entities;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Account
{
	public class QueryPermissionsViewModel
	{
		public GoogleAuthViewModel Auth { get; set; }
		public string id;
	}

	public class QueryPermissionsOutViewModel:APIDataModel
	{
		public IDictionary<string, PermissionRegion> Data { get; set; }
	}
}
