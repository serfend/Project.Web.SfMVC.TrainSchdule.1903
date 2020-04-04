using BLL.Helpers;
using DAL.DTO.User;
using System.Collections.Generic;

namespace TrainSchdule.ViewModels.Company
{
	/// <summary>
	///
	/// </summary>
	public class CompanyManagerViewModel : ApiResult
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