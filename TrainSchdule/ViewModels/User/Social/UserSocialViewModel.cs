using BLL.Helpers;
using DAL.DTO.User.Social;
using DAL.Entities;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.Settle;
using System;
using System.ComponentModel.DataAnnotations;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	///
	/// </summary>
	public class UserSocialViewModel : ApiResult
	{
		/// <summary>
		/// 社会情况信息
		/// </summary>
		public SocialDto Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserSocialSettleModefyViewModel : GoogleAuthViewModel
	{
		/// <summary>
		///
		/// </summary>
		public UserSocialSettleDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserSocialSettleDataModel
	{
		/// <summary>
		/// 用户id
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		///
		/// </summary>

		public SettleDto Settle { get; set; }
	}
}