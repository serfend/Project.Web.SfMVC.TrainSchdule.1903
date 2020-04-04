using BLL.Helpers;
using System;
using System.Collections.Generic;
using TrainSchdule.Extensions;

namespace TrainSchdule.ViewModels.System
{
	/// <summary>
	///
	/// </summary>
	public class APIResponseIdViewModel : ApiResult
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
		public APIResponseIdViewModel(Guid id, ApiResult message)
		{
			Data = new ApiResponseDataModel()
			{
				Id = id
			};
			Status = message.Status;
			Message = message.Message;
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