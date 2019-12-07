using DAL.Entities.UserInfo;
using System.Collections.Generic;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Account
{
	/// <summary>
	/// 移除多个用户
	/// </summary>
	public class UserRemoveMutiViewMode:GoogleAuthViewModel
	{
		public UserRemoveMutiDataMode Data { get; set; }
	}
	public class UserRemoveMutiDataMode
	{
		public IEnumerable<string> Id { get; set; }
	}
	/// <summary>
	///  移除单个用户
	/// </summary>
	public class UserRemoveViewModel: IdSubmitViewModel
	{
		
	}
	/// <summary>
	/// 
	/// </summary>
	public class UserApplicationViewModel : IdSubmitViewModel
	{
		public UserApplicationInfo Data { get; set; }
	}
}
