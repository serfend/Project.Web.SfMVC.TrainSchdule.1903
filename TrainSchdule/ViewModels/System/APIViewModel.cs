using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.Web.ViewModels
{

	public  class APIDataModel
	{
		[JsonProperty("status")]
		public int Code { get; set; }
		[JsonProperty("message")]
		public string Message { get; set; }
	}

	public class APIResponseIdViewModel: APIDataModel
	{
		public Guid Id { get; set; }

		public APIResponseIdViewModel(Guid id, Status message)
		{
			this.Id = id;
			this.Code = message.status;
			this.Message = message.message;
		}
	}

	public class APIResponseModelStateErrorViewModel : ModelStateExceptionViewModel
	{
		public Guid Id { get; set; }
		public APIResponseModelStateErrorViewModel(Guid id,ModelStateDictionary state) : base(state)
		{
			this.Id = id;
		}
	}
}
