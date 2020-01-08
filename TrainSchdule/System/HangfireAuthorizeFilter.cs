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
	public class HangfireAuthorizeFilter : IDashboardAuthorizationFilter
	{
		public bool Authorize([NotNull] DashboardContext context)
		{
			var httpcontext = context.GetHttpContext();
			var auth = httpcontext.Request.Cookies?["Auth"];
			return auth == "199500616";
		}
	}
}
