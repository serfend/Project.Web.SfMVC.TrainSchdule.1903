using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Account
{
	/// <summary>
	/// 用户指定角色
	/// </summary>
	public class UserRalteRoleViewModel: GoogleAuthViewModel
	{
		/// <summary>
		/// 被授权方
		/// </summary>
		public string User { get; set; }
		/// <summary>
		/// 角色名称
		/// </summary>
		public string Role { get; set; }
		/// <summary>
		/// 是否删除
		/// </summary>
		public bool IsRemove { get; set; }
	}
	/// <summary>
	/// 修改密码
	/// </summary>
	public class ModifyPasswordViewModel : GoogleAuthViewModel
	{
		/// <summary>
		/// 用户id
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// 老密码
		/// </summary>
		[Required]
		public string OldPassword { get; set; }

		/// <summary>
		/// 输入密码
		/// </summary>
		[Required]
		public string NewPassword { get; set; }

		/// <summary>
		/// 二次输入密码
		/// </summary>
		[Required]
		public string ConfirmNewPassword { get; set; }
	}
}