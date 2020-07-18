using BLL.Helpers;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Hosting;
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
				About = model.About
			};
			return r;
		}
	}
}