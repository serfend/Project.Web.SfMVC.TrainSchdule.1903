using System;
using System.Collections.Generic;
using BLL.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels
{
	public static class ModelStateExtensions
	{
		public static ModelStateExceptionViewModel ToModel(this ModelStateDictionary model) => new ModelStateExceptionViewModel(model);
	}
	/// <summary>
	/// 
	/// </summary>
	public class ModelStateExceptionViewModel : ApiResult
	{
		/// <summary>
		/// 
		/// </summary>
		public ModelStateExceptionDataModel Data { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="state"></param>
		public ModelStateExceptionViewModel(ModelStateDictionary state)
		{
			Data = new ModelStateExceptionDataModel { List = state.AllModelStateErrors() };
			Status = -1404;
			Message = "数据格式错误";
		}
	}

	[global::System.Serializable]
	public class ModelStateException : Exception
	{
		private ModelStateExceptionViewModel model;

		public ModelStateExceptionViewModel Model { get => model; set => model = value; }

		public ModelStateException(ModelStateExceptionViewModel model) : this(null, model) { }
		public ModelStateException(string message, ModelStateExceptionViewModel model) : base(message)
		{
			this.Model = model;
		}
		public ModelStateException(string message) : base(message) { }
		public ModelStateException(string message, global::System.Exception inner) : base(message, inner) { }
		protected ModelStateException(
		  global::System.Runtime.Serialization.SerializationInfo info,
		  global::System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
	/// <summary>
	/// 
	/// </summary>
	public class ModelStateExceptionDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<ShowError> List { get; set; }
	}
}
