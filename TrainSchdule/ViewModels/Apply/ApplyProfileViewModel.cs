using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;
using Newtonsoft.Json;
using TrainSchdule.Web.ViewModels;

namespace TrainSchdule.ViewModels.Apply
{
	public class ApplyProfileViewModel:APIDataModel
	{
		public ApplyProfileDataModel Data { get; set; }
	}

	public class ApplyProfileDataModel
	{
		[JsonProperty("applies")]
		public IEnumerable<ApplyDTO> Applies { get; set; }
	}

	public class ApplyDetailViewModel : APIDataModel
	{
		[JsonProperty("data")]
		public ApplyAllDataDTO Data { get; set; }
	}
}
