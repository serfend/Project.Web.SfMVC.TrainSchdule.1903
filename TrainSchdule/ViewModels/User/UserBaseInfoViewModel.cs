using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities.UserInfo;
using Newtonsoft.Json;
using TrainSchdule.ViewModels;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.User
{
	public class UserBaseInfoViewModel:APIDataModel
	{
		public UserBaseInfoDataModel Data { get; set; }
	}
	public class UserBaseInfoDataModel
	{
		public string Id { get; set; }
		public string RealName { get; set; }
		public string Avatar { get; set; }
		public GenderEnum Gender { get; set; }
		public bool PrivateAccount { get; set; }
	}

	public static class UserBaseInfoExtensions
	{
		public static UserBaseInfoDataModel ToModel(this UserBaseInfo model,string userid)
		{
			return  new UserBaseInfoDataModel()
			{
				Id = userid,
				Avatar = model.Avatar ?? (model.Gender == GenderEnum.Female ? @"\images\defaults\def-female-logo.png" : @"\images\defaults\def-male-logo.png"),
				Gender = model.Gender,
				PrivateAccount = model.PrivateAccount,
				RealName = model.RealName
			};
		}
	}
}
