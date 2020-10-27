using BLL.Interfaces;
using BLL.Services;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.System
{
	/// <summary>
	///
	/// </summary>
	public class HangfireAuthorizeFilter : IDashboardAuthorizationFilter
	{
		private string AuthCode = "XJXT@1994#00801";

		public HangfireAuthorizeFilter(string authCode)
		{
			if (authCode != null)
				AuthCode = authCode;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public bool Authorize([NotNull] DashboardContext context)
		{
			var httpcontext = context.GetHttpContext();
			var auth = httpcontext.Request.Cookies?["Auth"];

			return auth == AuthCode;
		}
	}
}