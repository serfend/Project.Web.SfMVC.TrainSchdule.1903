using DAL.Entities.UserInfo;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Account
{
	/// <summary>
	/// 
	/// </summary>
	public class UserRemoveViewModel : GoogleAuthViewModel
	{
		/// <summary>
		/// 
		/// </summary>
		public UserRemoveDataModel Data { get; set; }
	}
	/// <summary>
	/// 
	/// </summary>
	public class UsersRemoveViewModel : GoogleAuthViewModel
	{
		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<UserRemoveDataModel> Data { get; set; }
	}
	/// <summary>
	///  移除单个用户
	/// </summary>
	public class UserRemoveDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		[Required(ErrorMessage ="用户名未填写")]
		public string Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[Required(ErrorMessage ="移除原因未填写")]
		public string Reason { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserApplicationViewModel : IdSubmitViewModel
	{
		/// <summary>
		///
		/// </summary>
		public UserApplicationInfo Data { get; set; }
	}
}