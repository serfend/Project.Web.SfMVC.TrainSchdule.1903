using BLL.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.Controllers.Account
{
	/// <summary>
	///
	/// </summary>
	public partial class AccountController
	{
		/// <summary>
		/// 修改第三方账号请求
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public IActionResult ThirdpardRequire()
		{
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 确认修改第三方账号
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public IActionResult ThirdpardConfirm()
		{
			return new JsonResult(ActionStatusMessage.Success);
		}
	}
}