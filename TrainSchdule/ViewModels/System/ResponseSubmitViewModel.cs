using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TrainSchdule.ViewModels.System
{

	public class APIResponseIdViewModel : APIDataModel
	{
		public Guid Id { get; set; }

		public APIResponseIdViewModel(Guid id, Status message)
		{
			this.Id = id;
			this.Code = message.status;
			this.Message = message.message;
		}
	}
	public class APIResponseResultViewModel : APIDataModel
	{
		public string Result { get; set; }

		public APIResponseResultViewModel(string result, Status message)
		{
			this.Result = result;
			this.Code = message.status;
			this.Message = message.message;
		}
	}
	public class APIResponseModelStateErrorViewModel : ModelStateExceptionViewModel
	{
		public Guid Id { get; set; }
		public APIResponseModelStateErrorViewModel(Guid id, ModelStateDictionary state) : base(state)
		{
			this.Id = id;
		}
	}
}
