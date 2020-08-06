using BLL.Helpers;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.DiyInfo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	///
	/// </summary>
	public class UserDiyInfoViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public UserDiyInfoDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserDiyInfoModefyModel : GoogleAuthViewModel
	{
		/// <summary>
		///
		/// </summary>
		public UserDiyInfoDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserDiyInfoDataModel
	{
		/// <summary>
		///
		/// </summary>
		public string About { get; set; }

		/// <summary>
		///
		/// </summary>
		public string Avatar { get; set; }

		/// <summary>
		/// 第三方账号列表
		/// </summary>
		public IEnumerable<ThirdpardAccount> ThirdpardAccounts { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public static class UserDiyInfoExtension
	{
		/// <summary>
		/// 获取用户自定义信息
		/// </summary>
		/// <param name="model"></param>
		/// <param name="user">需要传入用户本身</param>
		/// <returns></returns>
		public static UserDiyInfoDataModel ToViewModel(this UserDiyInfo model, DAL.Entities.UserInfo.User user)
		{
			return new UserDiyInfoDataModel()
			{
				About = model.About,
				Avatar = model.Avatar?.Id.ToString(),
				ThirdpardAccounts = model.ThirdpardAccount
			};
		}

		/// <summary>
		/// 将传入信息转换为UserDiyInfo
		/// </summary>
		/// <param name="model"></param>
		/// <param name="db"></param>
		/// <returns></returns>
		public static UserDiyInfo ToModel(this UserDiyInfoDataModel model, DbSet<ThirdpardAccount> db)
		{
			var r = new UserDiyInfo()
			{
				About = model.About,
				ThirdpardAccount = model.ThirdpardAccounts
				.Select(i => db.FirstOrDefault(a => a.Id == i.Id))
				.Where(i => i != null)
			};
			return r;
		}
	}
}