using DAL.Entities.UserInfo;
using System.Collections.Generic;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Account
{
	/// <summary>
	/// 移除多个用户
	/// </summary>
	public class UserRemoveMutiViewMode : GoogleAuthViewModel
	{
		/// <summary>
		///
		/// </summary>
		public UserRemoveMutiDataMode Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserRemoveMutiDataMode
	{
		/// <summary>
		///
		/// </summary>
		public IEnumerable<string> Id { get; set; }
	}

	/// <summary>
	///  移除单个用户
	/// </summary>
	public class UserRemoveViewModel : IdSubmitViewModel
	{
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