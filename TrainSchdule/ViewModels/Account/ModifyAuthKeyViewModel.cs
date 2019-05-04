using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Account
{
	public class ModifyAuthKeyViewModel
	{
		public GoogleAuthViewModel Auth { get; set; }
		public string NewKey { get; set; }
	}
}
