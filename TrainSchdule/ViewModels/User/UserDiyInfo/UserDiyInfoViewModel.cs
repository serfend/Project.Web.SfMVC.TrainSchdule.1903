using BLL.Extensions;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.User
{
	public class UserDiyInfoViewModel: ApiDataModel
	{
		public UserDiyInfoDataModel Data { get; set; }
	}
public class UserDiyInfoModefyModel:GoogleAuthViewModel
	{
		public UserDiyInfoDataModel Data { get; set; }
	}	
 public class UserDiyInfoDataModel
	{
		public string About { get; set; }
		public string Avatar { get; set; }
	}

	public static class UserDiyInfoExtension
	{
		/// <summary>
		/// 获取用户自定义信息
		/// </summary>
		/// <param name="model"></param>
		/// <param name="user">需要传入用户本身</param>
		/// <returns></returns>
		public static UserDiyInfoDataModel ToViewModel (this UserDiyInfo model,DAL.Entities.UserInfo.User user,IHostingEnvironment env)
		{
			return new UserDiyInfoDataModel()
			{
				About = model.About,
				Avatar = model.Avatar?.Id.ToString()
			};
		}
		/// <summary>
		/// 将传入信息转换为UserDiyInfo
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static UserDiyInfo ToModel(this UserDiyInfoDataModel model)
		{
			var r = new UserDiyInfo()
			{
				About=model.About
			};
			return r;
		}
	}
}
