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
