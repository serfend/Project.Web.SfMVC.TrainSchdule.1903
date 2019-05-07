using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TrainSchdule.Extensions;

namespace TrainSchdule.ViewModels.System
{

	public class APIResponseIdViewModel : APIDataModel
	{
		public APIResponseDataModel Data { get; set; }

		public APIResponseIdViewModel(Guid id, Status message)
		{
			Data =new APIResponseDataModel()
			{
				Id = id
			};
			this.Code = message.status;
			this.Message = message.message;
		}
	}

	public class APIResponseDataModel
	{
		public Guid Id { get; set; }
	}


	public class APIResponseModelStateErrorViewModel : APIDataModel
	{
		public ModelStateResponseExceptionDataModel Data { get; set; }

		public APIResponseModelStateErrorViewModel(Guid id,ModelStateDictionary state)
		{
			Data = new ModelStateResponseExceptionDataModel { List = state.AllModelStateErrors() ,Id=id};
			this.Code = -2;
			this.Message = "成功,数据格式错误";
		}
	}

	public class ModelStateResponseExceptionDataModel
	{
		public IEnumerable<ShowError> List { get; set; }
		public Guid Id { get; set; }
	}
}
