using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.BLL.DTO.UserInfo;
using TrainSchdule.Web.ViewModels;

namespace TrainSchdule.ViewModels.User
{
	public class UserDetailViewModel:APIDataModel
	{
		public UserDetailsDTO Data { get; set; }
	}
}
