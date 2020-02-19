using BLL.Helpers;
using DAL.Entities.Game_r3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Game
{
	public class UsersViewModel:ApiResult
	{
		public UsersDataModel Data { get; set; }
	}
	public class UsersDataModel
	{
		public IEnumerable<UserInfo> Users { get; set; }
	}
}
