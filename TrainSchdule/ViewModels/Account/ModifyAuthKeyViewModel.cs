using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Account
{
	public class ModifyAuthKeyViewModel:GoogleAuthViewModel
	{
		/// <summary>
		/// 新的授权码
		/// </summary>
		public string NewKey { get; set; }
	}
}
