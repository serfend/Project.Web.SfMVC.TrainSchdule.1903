using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.BLL.Helpers;
using TrainSchdule.BLL.Interfaces;
using TrainSchdule.WEB.Extensions;

namespace TrainSchdule.WEB.Controllers
{
	[Route("[controller]/[action]")]
    public class UsersController : Controller
    {
        #region Fields

        private readonly IUsersService _usersService;
        private readonly ICurrentUserService _currentUserService;

        private bool _isDisposed;

        #endregion

        #region .ctors

        public UsersController(IUsersService usersService, ICurrentUserService currentUserService)
        {
            _usersService = usersService;
            _currentUserService = currentUserService;
        }

		#endregion

		#region Logic
		[HttpGet,Route("{username}")]
        public ActionResult Details(string username)
        {
            var item = _usersService.Get(username).ToViewModel();

            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUser = _currentUserService.CurrentUserDTO.ToViewModel();
            }

            return View(item);
        }
		

        [HttpGet]
        public IActionResult Info(string username=null)
        {
	        if(!User.Identity.IsAuthenticated)return new JsonResult(ActionStatusMessage.AccountAuth_Invalid);
			username =username.IsNullOrEmpty() ? _currentUserService.CurrentUserDTO.UserName:username;
			var item=_usersService.Get(username);
			if (item.Privilege>_currentUserService.CurrentUser.Privilege)return new JsonResult(ActionStatusMessage.AccountAuth_Forbidden);
			return new JsonResult(item);
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