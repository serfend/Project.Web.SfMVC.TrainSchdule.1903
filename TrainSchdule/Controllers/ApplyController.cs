using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.BLL.Interfaces;
using TrainSchdule.ViewModels.Apply;

namespace TrainSchdule.Web.Controllers
{
	[Route("[controller]/[action]")]
	public class ApplyController: Controller
	{
		#region filed
		private readonly IUsersService _usersService;
		private readonly ICurrentUserService _currentUserService;
		private readonly IApplyService _applyService;

		private bool _isDisposed;

		public ApplyController(IUsersService usersService, ICurrentUserService currentUserService, IApplyService applyService)
		{
			_usersService = usersService;
			_currentUserService = currentUserService;
			_applyService = applyService;
		}

		#endregion

		#region Logic
		[HttpPost]
		public async Task<IActionResult> Submit(ApplySubmitViewModel model)
		{
			var user = _currentUserService.CurrentUser;
			var item=new Apply()
			{
				Address = user.Address,
				Company = user.Company.Path,
				Create = DateTime.Now,
				From = user,
				Status = AuditStatus.Auditing,
				xjlb	= model.Param.xjlb

			};
			

			var apply=await _applyService.CreateAsync(item);
			
			return  new JsonResult(null);
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
