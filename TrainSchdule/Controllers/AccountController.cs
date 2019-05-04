using BLL.Helpers;
using BLL.Interfaces;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.Account;
using TrainSchdule.ViewModels.System;
using TrainSchdule.WEB.Extensions;

namespace TrainSchdule.WEB.Controllers
{
	[Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        #region Fields

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IUsersService _usersService;
        private readonly IVerifyService _verifyService;
        private readonly IGoogleAuthService _authService;

        private bool _isDisposed;

        #endregion

        #region .ctors

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger,
            IUsersService usersService, IVerifyService verifyService, IGoogleAuthService authService) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _usersService = usersService;
            _verifyService = verifyService;
            _authService = authService;
        }

        #endregion

        #region Properties

        [TempData]
        public string ErrorMessage { get; set; }

		#endregion



		#region Rest

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> ConfirmEmail(string userId, string code)
		{
			if (userId == null || code == null)
			{
				return new JsonResult(ActionStatusMessage.Fail);
			}
			var user = await _userManager.FindByIdAsync(userId) ??
			           throw new ApplicationException($"无法加载当前用户信息 '{userId}'.");

			var result = await _userManager.ConfirmEmailAsync(user, code);
			return View(result.Succeeded ? "ConfirmEmail" : "Error");
		}
		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			_logger.LogInformation("User logged out.");

			return new JsonResult(ActionStatusMessage.Success);
		}

		[HttpPost]
		[AllowAnonymous]
		public IActionResult AuthKey(ModifyAuthKeyViewModel model)
		{
			if(!_authService.Verify(model.Auth.Code,model.Auth.AuthByUserID))return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var success = _usersService.Edit(model.Auth.AuthByUserID, (x) =>
			{
				x.Application.AuthKey = model.NewKey;
			});
			if(!success)return new JsonResult(ActionStatusMessage.User.NotExist);
			
			return new JsonResult(ActionStatusMessage.Success);
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult AuthKey()
		{
			if(!User.Identity.IsAuthenticated)return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			var qrCoder = new BLL.Helpers.QRCoder();
			_authService.InitCode();
			var img = qrCoder.GenerateBytes(_authService.Url);
			HttpContext.Response.Cookies.Append("key",_authService.Code().ToString());
			return new FileContentResult(img,"image/png");
		}
		
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				if (!model.Verify.Verify)
				{
					return new JsonResult(ActionStatusMessage.Account.Auth.Verify.Invalid);
				}

				var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
				if (result.Succeeded)
				{
					_logger.LogInformation($"用户登录:{model.UserName}");
					return new JsonResult(ActionStatusMessage.Success);
				}
				else if (result.RequiresTwoFactor)
				{
					return new JsonResult(ActionStatusMessage.Account.Login.AuthException);
				}
				else if (result.IsLockedOut)
				{
					_logger.LogWarning("账号异常");
					return new JsonResult(ActionStatusMessage.Account.Login.AuthBlock);
				}
				else
				{
					return new JsonResult(ActionStatusMessage.Account.Login.AuthAccountOrPsw);
				}
			}
			else
			{
				return new JsonResult(new Status(ActionStatusMessage.Fail.status, JsonConvert.SerializeObject(ModelState.AllModelStateErrors())));
			}
		}
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Register([FromBody]UserCreateViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			if (!model.Verify.Verify)return new JsonResult(ActionStatusMessage.Account.Auth.Verify.Invalid);
			
			if (!_authService.Verify(model.Auth.Code, model.Auth.AuthByUserID)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);

			var user = await _usersService.CreateAsync(model.Data);
			if (user == null)
			{
				return new JsonResult(ActionStatusMessage.Account.Register.UserExist);
			}
			_logger.LogInformation($"新的用户创建:{user.UserName}");

			var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
			await _emailSender.SendEmailConfirmationAsync(user.Email, callbackUrl);

			//await _signInManager.SignInAsync(user, isPersistent: false);
			return new JsonResult(ActionStatusMessage.Success);
		}


        #endregion

        #region Disposing

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _userManager.Dispose();
                    _usersService.Dispose();
                }

                _isDisposed = true;

                base.Dispose(disposing);
            }
        }

        #endregion
    }
}
