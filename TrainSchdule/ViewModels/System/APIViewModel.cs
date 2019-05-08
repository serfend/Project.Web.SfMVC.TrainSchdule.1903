using Newtonsoft.Json;

namespace TrainSchdule.ViewModels.System
{

	/// <summary>
	/// API返回状态
	/// </summary>
	public  class ApiDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("status")]
		public int Code { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("message")]
		public string Message { get; set; }
	}

}
