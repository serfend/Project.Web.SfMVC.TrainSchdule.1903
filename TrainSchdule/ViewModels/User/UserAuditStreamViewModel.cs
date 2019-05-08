using System.Collections.Generic;
using DAL.DTO.Company;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	/// 
	/// </summary>
	public class UserAuditStreamViewModel:ApiDataModel
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
