using System.ComponentModel.DataAnnotations;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Account
{
	/// <summary>
	/// 修改权限
	/// </summary>
	public class ModefyPermissionsViewModel : GoogleAuthViewModel
	{
		/// <summary>
		/// 指定id
		/// </summary>
		public string Id { get; set; }
		/// <summary>
		/// 新的权限
		/// </summary>
		public string NewPermission { get; set; }

	}
	/// <summary>
	/// 修改密码
	/// </summary>
	public class ModefyPasswordViewModel : ModefyPermissionsViewModel
	{
		/// <summary>
		/// 老密码
		/// </summary>
		[Required]
		public string OldPassword { get; set; }

		/// <summary>
		/// 输入密码
		/// </summary>
		[Required]
		[StringLength(32, ErrorMessage = "非法的{0}", MinimumLength = 8)]

		public string NewPassword { get; set; }
		/// <summary>
		/// 二次输入密码
		/// </summary>
		[Required]
		[Compare(nameof(NewPassword), ErrorMessage = "两次输入的密码不一致")]
		public string ConfirmNewPassword { get; set; }
	}
}
