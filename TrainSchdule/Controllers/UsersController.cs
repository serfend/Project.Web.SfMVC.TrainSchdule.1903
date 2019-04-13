using Microsoft.AspNetCore.Mvc;
using TrainSchdule.BLL.Helpers;
using TrainSchdule.BLL.Interfaces;
using TrainSchdule.WEB.Extensions;

namespace TrainSchdule.WEB.Controllers
{
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

        [HttpGet, Route("users/{userName}")]
        public ActionResult Details(string userName)
        {
            var item = _usersService.Get(userName).ToViewModel();

            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUser = _currentUserService.CurrentUserDTO.ToViewModel();
            }

            return View(item);
        }
		

        [HttpGet]
        public IActionResult UserInfo(string username)
        {
	        if(!User.Identity.IsAuthenticated)return new JsonResult(ActionStatusMessage.AccountAuth_Invalid);

	        var item = _usersService.Get(username);
			if(item.Privilege>_currentUserService.CurrentUser.Privilege)return new JsonResult(ActionStatusMessage.AccountAuth_Forbidden);
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