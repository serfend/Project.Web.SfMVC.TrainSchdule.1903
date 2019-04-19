using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrainSchdule.Web.ViewModels;

namespace TrainSchdule.ViewModels.Apply
{
	public class ApplyCreatedViewModel:APIDataModel
	{
		public ApplyCreatedDataModel Data { get; set; }
	}

	public class ApplyCreatedDataModel
	{
		[JsonProperty("id")]
		public Guid Id { get; set; }
	}
}
