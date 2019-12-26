using BLL.Helpers;
using DAL.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	/// 用户休假信息
	/// </summary>
	public class UserVocationInfoViewModel : ApiResult
	{
		/// <summary>
		/// 
		/// </summary>
		public UserVocationInfoVDto Data{get;set;}
	}
	


}
