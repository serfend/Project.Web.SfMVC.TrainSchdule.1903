using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.Web.ViewModels.Company
{
	public class CompanyViewModel:APIViewModel
	{
		public string Name { get; set; }
		public string ParentPath { get; set; }
	}
}
