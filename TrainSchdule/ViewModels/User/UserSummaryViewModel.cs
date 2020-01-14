using BLL.Helpers;
using DAL.DTO.User;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.User
{
	public class UserSummariesViewModel : ApiResult
	{
		public UserSummariesDataModel Data { get; set; }
	}
	public class UserSummariesDataModel
	{
		public IEnumerable<UserSummaryDto> List { get; set; }
	}
	/// <summary>
	/// 
	/// </summary>
	public class UserSummaryViewModel: ApiResult
	{
		/// <summary>
		/// 
		/// </summary>
		public UserSummaryDto Data { get; set; }
	}

}
