using Newtonsoft.Json;

namespace TrainSchdule.ViewModels.System
{

	public  class APIDataModel
	{
		[JsonProperty("status")]
		public int Code { get; set; }
		[JsonProperty("message")]
		public string Message { get; set; }
	}

}
