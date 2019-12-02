using BLL.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;

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
		public ApiDataModel() { }

		public ApiDataModel(int code, string message)
		{
			Code = code;
			Message = message;
		}
		public ApiDataModel(Status status) : this(status.status,status.message) { }
	}
	/// <summary>
	/// 批量请求情况回复
	/// </summary>
	public class ResponseStatusOrModelExceptionViweModel:ApiDataModel
	{
		public ResponseStatusOrModelExceptionViweModel(Status status) : base(status.status, status.message) { }
		/// <summary>
		/// 返回键对应的错误
		/// </summary>
		public Dictionary<string, Status> StatusException { get; set; }
		/// <summary>
		/// 键对应的格式错误
		/// </summary>
		public Dictionary<string,ModelStateExceptionDataModel> ModelStateException { get; set; }
	}
}
