using BLL.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TrainSchdule.ViewModels.System
{
	/// <summary>
	/// 批量请求情况回复
	/// </summary>
	public class ResponseStatusOrModelExceptionViweModel:ApiResult
	{
		public ResponseStatusOrModelExceptionViweModel(ApiResult status) : base(status.Status, status.Message) { }
		/// <summary>
		/// 返回键对应的错误
		/// </summary>
		public Dictionary<string, ApiResult> StatusException { get; set; }
		/// <summary>
		/// 键对应的格式错误
		/// </summary>
		public Dictionary<string,ModelStateExceptionDataModel> ModelStateException { get; set; }
	}
}
