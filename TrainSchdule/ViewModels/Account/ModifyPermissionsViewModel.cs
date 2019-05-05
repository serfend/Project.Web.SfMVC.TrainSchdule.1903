using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Account
{
	public class ModifyPermissionsViewModel
	{
		public GoogleAuthViewModel Auth { get; set; }
		public string id;
		public string NewPermission { get; set; }

	}
}
