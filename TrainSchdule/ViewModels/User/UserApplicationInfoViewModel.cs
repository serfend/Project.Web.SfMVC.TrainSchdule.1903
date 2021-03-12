using BLL.Helpers;
using DAL.Entities.UserInfo;
using System;
using System.ComponentModel.DataAnnotations;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	///
	/// </summary>
	public class UserApplicationInfoViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public UserApplicationDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserApplicationDataModel
	{
		/// <summary>
		/// 用户id
		/// </summary>
		[StringLength(32, ErrorMessage = "用户名不可少于7位", MinimumLength = 7)]
		[Required]
		public string UserName { get; set; }

		/// <summary>
		///
		/// </summary>
		public string InvitedBy { get; set; }

		/// <summary>
		///
		/// </summary>
		public DateTime? Create { get; set; }

		/// <summary>
		///
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// 用户状态
		/// </summary>
		public AccountStatus AccountStatus { get; set; }

		/// <summary>
		/// 当状态为封禁时需要有此字段
		/// </summary>
		public DateTime StatusBeginDate { get; set; }

		/// <summary>
		/// 当状态为封禁时需要有此字段
		/// </summary>
		public DateTime StatusEndDate { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public static class UserApplicationInfoExtensions
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		public static UserApplicationDataModel ToModel(this UserApplicationInfo model, DAL.Entities.UserInfo.User user)
		{
			return new UserApplicationDataModel()
			{
				UserName = user.Id,
				Create = model.Create,
				Email = model.Email,
				InvitedBy = model.InvitedBy,
				AccountStatus = user.AccountStatus,
				StatusBeginDate = user.StatusBeginDate,
				StatusEndDate = user.StatusEndDate,
			};
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <param name="inviteBy"></param>
		/// <returns></returns>
		public static UserApplicationInfo ToModel(this UserApplicationDataModel model, string inviteBy)
		{
			return new UserApplicationInfo()
			{
				Email = model.Email,
				// fixbug do not beleve user data: InvitedBy=model.InvitedBy,
				InvitedBy = inviteBy
			};
		}
	}
}