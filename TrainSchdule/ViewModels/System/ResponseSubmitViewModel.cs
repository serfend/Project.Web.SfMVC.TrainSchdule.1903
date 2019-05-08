using System;
using System.Collections.Generic;
using BLL.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TrainSchdule.Extensions;

namespace TrainSchdule.ViewModels.System
{

	/// <summary>
	/// 
	/// </summary>
	public class APIResponseIdViewModel : ApiDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public ApiResponseDataModel Data { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="message"></param>
		public APIResponseIdViewModel(Guid id, Status message)
		{
			Data =new ApiResponseDataModel()
			{
				Id = id
			};
			Code = message.status;
			Message = message.message;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class ApiResponseDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public Guid Id { get; set; }
	}


	/// <summary>
	/// 
	/// </summary>
	public class ApiResponseModelStateErrorViewModel : ApiDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public ModelStateResponseExceptionDataModel Data { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="state"></param>
		public ApiResponseModelStateErrorViewModel(Guid id,ModelStateDictionary state)
		{
			Data = new ModelStateResponseExceptionDataModel { List = state.AllModelStateErrors() ,Id=id};
			Code = -2;
			Message = "成功,数据格式错误";
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class ModelStateResponseExceptionDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<ShowError> List { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Guid Id { get; set; }
	}
}
