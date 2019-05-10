using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Account
{
	public class UserIdByCidViewModel:ApiDataModel
	{
		public UserIdByCidDataModel Data { get; set; }
	}

	public class UserIdByCidDataModel
	{
		public string Id { get; set; }
	}
}
