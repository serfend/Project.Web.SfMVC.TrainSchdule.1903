using System.Collections.Generic;
using BLL.Helpers;
using DAL.DTO.User;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Company
{
	/// <summary>
	/// 
	/// </summary>
	public class CompanyManagerViewModel:ApiResult
	{
		/// <summary>
		/// 
		/// </summary>
		public CompanyManagerDataModel Data { get; set; }
	}

	/// <summary>
	/// 
	/// </summary>
	public class CompanyManagerDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<UserSummaryDto> List { get; set; }
	}
}
