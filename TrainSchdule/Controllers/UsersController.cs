using BLL.Helpers;
using BLL.Interfaces;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.ViewModels.User;

namespace TrainSchdule.WEB.Controllers
{
	[Authorize]
	[Route("[controller]/[action]")]
    public class UsersController : Controller
    {
        #region Fields

        private readonly IUsersService _usersService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICompaniesService _companiesService;

        private bool _isDisposed;

        #endregion

        #region .ctors

        public UsersController(IUsersService usersService, ICurrentUserService currentUserService, ICompaniesService companiesService)
        {
            _usersService = usersService;
            _currentUserService = currentUserService;
            _companiesService = companiesService;
        }

		#endregion

		#region Logic
		

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Base(string id)
		{
			if (!User.Identity.IsAuthenticated) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			id = id.IsNullOrEmpty() ? _currentUserService.CurrentUser?.Id : id;
			var targetUser = _usersService.Get(id);
			if(targetUser==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			return  new JsonResult(new UserBaseInfoViewModel()
			{
				Data = targetUser.BaseInfo.ToModel()
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