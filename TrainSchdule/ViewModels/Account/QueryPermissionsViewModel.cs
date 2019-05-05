﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrainSchdule.ViewModels.Verify;
using TrainSchdule.Web.ViewModels;

namespace TrainSchdule.ViewModels.Account
{
	public class QueryPermissionsViewModel
	{
		public GoogleAuthViewModel Auth { get; set; }
		public string id;
	}

	public class QueryPermissionsOutViewModel:APIDataModel
	{
		public object Data { get; set; }
	}
}