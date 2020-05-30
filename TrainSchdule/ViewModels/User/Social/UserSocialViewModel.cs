using BLL.Helpers;
using DAL.DTO.User.Social;
using DAL.Entities;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.Settle;
using System;
using System.ComponentModel.DataAnnotations;

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
}