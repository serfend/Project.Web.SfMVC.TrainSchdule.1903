using System.Collections.Generic;
using BLL.Helpers;
using DAL.DTO.Company;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	/// 
	/// </summary>
	public class UserAuditStreamViewModel:ApiResult
	{
		/// <summary>
		/// 
		/// </summary>
		public UserAuditStreamDataModel Data { get; set; }
	}

	/// <summary>
	/// 
	/// </summary>
	public class UserAuditStreamDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<CompanyDto> List { get; set; }
	}

}
