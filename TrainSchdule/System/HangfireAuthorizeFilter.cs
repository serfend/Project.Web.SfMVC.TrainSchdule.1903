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
		private IGoogleAuthService googleAuthService;

		public HangfireAuthorizeFilter()
		{
			this.googleAuthService = new GoogleAuthService();
		}

		public bool Authorize([NotNull] DashboardContext context)
		{
			var httpcontext = context.GetHttpContext();
			var auth = httpcontext.Request.Cookies?["Auth"];
			if (auth != null) return new GoogleAuthDataModel()
			{
				AuthByUserID="root",
				Code=Convert.ToInt32(auth)
			}.Verify(googleAuthService);
			return (httpcontext.User.Identity.IsAuthenticated && httpcontext.User.Identity.Name == "1000000");
		}
	}
}
