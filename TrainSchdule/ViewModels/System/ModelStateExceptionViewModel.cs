using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TrainSchdule.Extensions;

namespace TrainSchdule.ViewModels
{
	public class ModelStateExceptionViewModel:APIDataModel
	{
		public ModelStateExceptionDataModel Data { get; set; }

		public ModelStateExceptionViewModel(ModelStateDictionary state)
		{
			Data = new ModelStateExceptionDataModel {List = state.AllModelStateErrors()};
			this.Code = -1;
			this.Message = "数据格式错误";
		}
	}

	public class ModelStateExceptionDataModel
	{
		public IEnumerable<ShowError> List { get; set; }
	}
}
