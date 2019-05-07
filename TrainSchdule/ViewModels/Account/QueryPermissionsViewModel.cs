using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrainSchdule.ViewModels;
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
