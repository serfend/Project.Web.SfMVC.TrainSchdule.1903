using BLL.Helpers;
using DAL.DTO.Company;
using System.Collections.Generic;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	///
	/// </summary>
	public class UserManageRangeViewModel : ApiResult
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