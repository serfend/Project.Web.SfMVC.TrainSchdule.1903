using BLL.Helpers;
using DAL.DTO.User;
using System.Collections.Generic;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	///
	/// </summary>
	public class UserSummariesViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public UserSummariesDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserSummariesDataModel
	{
		/// <summary>
		///
		/// </summary>
		public IEnumerable<UserSummaryDto> List { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserSummaryViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public UserSummaryDto Data { get; set; }
	}
}