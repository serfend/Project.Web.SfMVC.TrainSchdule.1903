using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Static
{
	public class LocationChildrenViewModel:APIDataModel
	{
		public LocationChildrenDataModel Data { get; set; }
	}

	public class LocationChildrenDataModel
	{
		public IEnumerable<LocationChildNodeDataModel> List { get; set; }
	}

	public class LocationChildNodeDataModel
	{
		public string Name { get; set; }
		public int Code { get; set; }
	}
}
