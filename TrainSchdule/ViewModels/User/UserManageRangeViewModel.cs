using System.Collections.Generic;
using DAL.DTO.Company;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	/// 
	/// </summary>
	public class UserManageRangeViewModel:ApiDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public UserManageRangeDataModel Data { get; set; }
	}
	/// <summary>
	/// 
	/// </summary>
	public class UserManageRangeDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<CompanyDto> List { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int TotalCount { get; set; }
	}
}
