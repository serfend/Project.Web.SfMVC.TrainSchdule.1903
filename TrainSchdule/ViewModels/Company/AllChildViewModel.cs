using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Company
{
	public class AllChildViewModel:APIDataModel
	{
		public AllChildDataModel Data { get; set; }
	}

	public class AllChildDataModel
	{
		public IEnumerable<CompanyChildDataModel> List { get; set; }
	}
	public class CompanyChildDataModel
	{
		public string Name { get; set; }
		public string Code { get; set; }
	}
}
