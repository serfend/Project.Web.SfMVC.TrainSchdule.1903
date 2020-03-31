using BLL.Helpers;
using DAL.Entities.Game_r3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Game
{
	/// <summary>
	///
	/// </summary>
	public class UsersViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public UsersDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UsersDataModel
	{
		/// <summary>
		///
		/// </summary>
		public IEnumerable<UserInfo> Users { get; set; }
	}
}