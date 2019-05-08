using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Static
{
	public class LocationViewModel:APIDataModel
	{
		public LocationDataModel Data { get; set; }
	}

	public class LocationDataModel
	{
		public string Name { get; set; }
		public string ShortName { get; set; }
		public int Code { get; set; }
		public int ParentCode { get; set; }
	}

}
