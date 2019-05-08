using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels
{
	
	
	public class ModelStateExceptionViewModel:APIDataModel
	{
		public ModelStateExceptionDataModel Data { get; set; }

		public ModelStateExceptionViewModel(ModelStateDictionary state)
		{
			Data = new ModelStateExceptionDataModel {List = state.AllModelStateErrors()};
			Code = -1;
			Message = "数据格式错误";
		}
	}

	public class ModelStateExceptionDataModel
	{
		public IEnumerable<ShowError> List { get; set; }
	}
}
