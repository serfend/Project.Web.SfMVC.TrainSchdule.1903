using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;
using Newtonsoft.Json;
using TrainSchdule.Web.ViewModels;

namespace TrainSchdule.ViewModels.Apply
{
	public class ApplyProfileViewModel:APIViewModel
	{
		[JsonProperty("applies")]
		public IEnumerable<ApplyDTO> Applies { get; set; }
	}

	public class ApplyDetailViewModel : APIViewModel
	{
		[JsonProperty("data")]
		public ApplyDTO Data { get; set; }
	}
}
