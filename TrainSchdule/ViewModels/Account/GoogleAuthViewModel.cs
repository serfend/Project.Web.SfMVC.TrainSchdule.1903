using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Account
{
	/// <summary>
	/// 通过<see cref="AuthUserName"/>找到当前授权用户的创建权限是否符合新建用户的单位
	/// 如符合，判断当前<see cref="AuthCode"/>和GoogleAuth授权码一致，若一致，则注册进行下一步
	/// </summary>
	public class GoogleAuthViewModel
	{
		[Required]
		public string AuthCode { get; set; }

		[Required]
		public string AuthUserName { get; set; }
	}
}
