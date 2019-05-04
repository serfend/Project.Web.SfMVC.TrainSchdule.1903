using BLL.Extensions;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.ViewModels.Apply;

namespace TrainSchdule.Web.Controllers
{
	[Authorize]
	[Route("[controller]/[action]")]
	public class ApplyController: Controller
	{
		#region filed
		private readonly IUsersService _usersService;
		private readonly ICurrentUserService _currentUserService;
		private readonly IApplyService _applyService;
		private readonly ICompaniesService _companiesService;
		private readonly IVerifyService _verifyService;

		private bool _isDisposed;

		public ApplyController(IUsersService usersService, ICurrentUserService currentUserService, IApplyService applyService, ICompaniesService companiesService, IVerifyService verifyService)
		{
			_usersService = usersService;
			_currentUserService = currentUserService;
			_applyService = applyService;
			_companiesService = companiesService;
			_verifyService = verifyService;
		}

		#endregion

		#region Logic

		[HttpGet]
		public IActionResult AllStatus()
		{
			return new JsonResult(new ApplyAuditStatusViewModel()
			{
				Data = new ApplyAuditStatusDataModel()
				{
					List = ApplyExtensions.StatusDic
				}
			});
		}

		#endregion

		#region Disposing

		protected override void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				if (disposing)
				{
					_usersService.Dispose();
					_currentUserService.Dispose();
				}

				_isDisposed = true;

				base.Dispose(disposing);
			}
		}

		#endregion
	}
}
