using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TrainSchdule.Web.ViewModels;

namespace TrainSchdule.ViewModels.Static
{
	public class VerifyGeneratedViewModel:APIViewModel
	{
		[JsonProperty("id")]
		public string Id { get; set; }
		[JsonProperty("posY")]
		public int PosY { get; set; }
	}
}
