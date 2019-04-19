using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TrainSchdule.Web.ViewModels
{
	public sealed class APIViewModel
	{
		[JsonProperty("status")]
		public int Code { get; set; }
		[JsonProperty("message")]
		public string Message { get; set; }
	}
	public  class APIDataModel
	{
		[JsonProperty("status")]
		public int Code { get; set; }
		[JsonProperty("message")]
		public string Message { get; set; }
	}
}
