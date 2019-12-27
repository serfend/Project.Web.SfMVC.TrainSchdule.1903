﻿using System;
using System.ComponentModel.DataAnnotations;
using BLL.Helpers;
using DAL.Entities.UserInfo;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	/// 
	/// </summary>
	public class UserApplicationInfoViewModel:ApiResult
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
		public string UserName { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string InvitedBy { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime?Create { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Email { get; set; }
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
		/// <returns></returns>
		public static UserApplicationDataModel ToModel(this UserApplicationInfo model,DAL.Entities.UserInfo.User user)
		{
			return new UserApplicationDataModel()
			{
				UserName=user.Id,
				Create = model.Create,
				Email = model.Email,
				InvitedBy = model.InvitedBy
			};
		}
		public static UserApplicationInfo ToModel(this UserApplicationDataModel model)
		{
			return new UserApplicationInfo()
			{
				Email = model.Email,
				InvitedBy=model.InvitedBy,
			};
		}
	}
}
