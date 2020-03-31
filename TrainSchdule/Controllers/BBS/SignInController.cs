using BLL.Helpers;
using BLL.Interfaces.BBS;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.Controllers.BBS
{
	/// <summary>
	/// 签到
	/// </summary>
	[Route("[controller]/[action]")]
	public class SignInController : Controller
	{
		private readonly ISignInServices signInServices;

		/// <summary>
		///
		/// </summary>
		/// <param name="signInServices"></param>
		public SignInController(ISignInServices signInServices)
		{
			this.signInServices = signInServices;
		}

		/// <summary>
		/// 签到
		/// </summary>
		/// <param name="signInId"></param>
		/// <returns></returns>
		public IActionResult SignIn(string signInId)
		{
			var lastInDaySignIn = signInServices.QuerySingle(signInId, DateTime.Today, DateTime.Now);
			if (lastInDaySignIn != null)
			{
				return new JsonResult(new ApiResult(143392, $"已签到过啦 {lastInDaySignIn.Date}"));
			}
			signInServices.SignIn(signInId);
			return new JsonResult(ActionStatusMessage.Success);
		}
	}
}