using BLL.Extensions;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.User;

namespace TrainSchdule.Extensions.Users
{
	/// <summary>
	/// 头像转前端
	/// </summary>
	public static class AvatarExtensions
	{
		/// <summary>
		/// 转为输出
		/// </summary>
		/// <param name="avatar"></param>
		/// <returns></returns>
		public static AvatarDataModel ToModel(this Avatar avatar)
		{
			return new AvatarDataModel()
			{
				Create = avatar?.CreateTime ?? DateTime.Now,
				Url = $"data:image/png;base64,{avatar?.Img?.ToBase64()}",
				Path = avatar?.FilePath
			};
		}
	}
}