using System.Collections.Generic;
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
	public class ModelStateExceptionViewModel:ApiDataModel
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
			Data = new ModelStateExceptionDataModel {List = state.AllModelStateErrors()};
			Code = -1;
			Message = "数据格式错误";
		}
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
