using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities.UserInfo;
using TrainSchdule.Web.ViewModels;

namespace TrainSchdule.ViewModels.User
{
	public class UserBaseInfoViewModel:APIDataModel
	{
		public UserBaseInfoDataModel Data { get; set; }
	}

	public class UserBaseInfoDataModel:UserBaseInfo
	{
		
	}

	public static class UserBaseInfoExtensions
	{
		public static UserBaseInfoDataModel ToModel(this UserBaseInfo model)
		{
			return  new UserBaseInfoDataModel()
			{
				Avatar = model.Avatar ?? (model.Gender == GenderEnum.Female ? @"\images\defaults\def-female-logo.png" : @"\images\defaults\def-male-logo.png"),
				Gender = model.Gender,
				PrivateAccount = model.PrivateAccount,
				RealName = model.RealName
			};
		}
	}
}
