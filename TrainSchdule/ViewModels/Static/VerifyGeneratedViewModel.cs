using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.Web.ViewModels;

namespace TrainSchdule.ViewModels.Static
{
	public class VerifyGeneratedViewModel:APIViewModel
	{
		public string id { get; set; }
		public int ypos { get; set; }
	}
}
