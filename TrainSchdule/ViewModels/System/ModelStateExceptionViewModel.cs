using BLL.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using TrainSchdule.Extensions;

namespace TrainSchdule.ViewModels
{
	/// <summary>
	///
	/// </summary>
	public static class ModelStateExtensions
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
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

	/// <summary>
	///
	/// </summary>
	[global::System.Serializable]
	public class ModelStateException : Exception
	{
		private ModelStateExceptionViewModel model;

		/// <summary>
		///
		/// </summary>
		public ModelStateExceptionViewModel Model { get => model; set => model = value; }

		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		public ModelStateException(ModelStateExceptionViewModel model) : this(null, model) { }

		/// <summary>
		///
		/// </summary>
		/// <param name="message"></param>
		/// <param name="model"></param>
		public ModelStateException(string message, ModelStateExceptionViewModel model) : base(message)
		{
			this.Model = model;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="message"></param>
		public ModelStateException(string message) : base(message) { }

		/// <summary>
		///
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public ModelStateException(string message, global::System.Exception inner) : base(message, inner) { }

		/// <summary>
		///
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
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