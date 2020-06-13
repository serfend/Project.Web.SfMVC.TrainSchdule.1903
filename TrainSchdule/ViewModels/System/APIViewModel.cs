using BLL.Helpers;
using System.Collections.Generic;

namespace TrainSchdule.ViewModels.System
{
	/// <summary>
	/// 批量请求情况回复
	/// </summary>
	public class ResponseStatusOrModelExceptionViweModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="status"></param>
		public ResponseStatusOrModelExceptionViweModel(ApiResult status) : base(status.Status, status.Message) { }

		/// <summary>
		/// 返回键对应的错误
		/// </summary>
		public Dictionary<string, ApiResult> StatusException { get; set; }

		/// <summary>
		/// 键对应的格式错误
		/// </summary>
		public Dictionary<string, ModelStateExceptionDataModel> ModelStateException { get; set; }
	}

	/// <summary>
	/// 任意 实体返回
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class EntityViewModel<T> : ApiResult where T : class
	{
		public EntityViewModel(T data)
		{
			Data = data;
		}

		/// <summary>
		///
		/// </summary>
		public T Data { get; set; }
	}
}