using System;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using TrainSchdule.BLL.Helpers;
using TrainSchdule.DAL.Entities;
using TrainSchdule.BLL.Interfaces;
using TrainSchdule.DAL.Interfaces;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.Account;
using TrainSchdule.WEB.Extensions;
using TrainSchdule.WEB.ViewModels.Account;

namespace TrainSchdule.WEB.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        #region Fields

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
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
            IUsersService usersService, IVerifyService verifyService, IGoogleAuthService authService, IUnitOfWork unitOfWork) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _usersService = usersService;
            _verifyService = verifyService;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Properties

        [TempData]
        public string ErrorMessage { get; set; }

        #endregion

        #region Logic

        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl )
        {
            ViewData["ReturnUrl"] = returnUrl;
            var result =(JsonResult) await Login(model);
            int code = ((Status) result.Value).status;
            if (code==ActionStatusMessage.Success.status)return RedirectToLocal(returnUrl);
			else if(code==ActionStatusMessage.Account.Login.AuthException.status)return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
            else if(code==ActionStatusMessage.Account.Login.AuthBlock.status)  return RedirectToAction(nameof(Lockout));

	        ModelState.AddModelError(string.Empty, $"登录失败:{((Status)result.Value).message}");
	        // If we got this far, something failed, redisplay form
            return View(model);
        }
		

		[HttpPost]
        public async Task<IActionResult> Logout()
        {
	        await _signInManager.SignOutAsync();
	        _logger.LogInformation("User logged out.");
	       
			return new JsonResult(ActionStatusMessage.Success);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var model = new LoginWith2faViewModel { RememberMe = rememberMe };
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"无法加载当前用户信息 '{_userManager.GetUserId(User)}'.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
		[AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var result =(JsonResult)await Register(model);
            var status = (Status) result.Value;
            if (status.status == ActionStatusMessage.Success.status)
            {
	            return RedirectToLocal(returnUrl);
            }else if (status.status == ActionStatusMessage.Account.Register.UserExist.status)
            {
	            ModelState.AddModelError(string.Empty, "此账号已被使用");
	            return View(model);
            }
            else
            {
	            return View(model);
            }
        }
		
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }
		

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                string userName = "";
                if (!string.IsNullOrEmpty(email))
                    userName = EmailToUserName(email);

                return View("ExternalLogin", new ExternalLoginViewModel { Email = email, UserName = userName });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync() ??
                    throw new ApplicationException("Error loading external login information during confirmation.");

                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId) ??
                throw new ApplicationException($"无法加载当前用户信息 '{userId}'.");

            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }
                
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "重置密码",
                   $": <a href='{callbackUrl}'>点击此处</a>重置密码");
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("安全码异常");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

		#endregion

		#region Rest

		[HttpPost]
		[AllowAnonymous]
		[Route("rest")]
		public IActionResult AuthKey(ModifyAuthKeyViewModel model)
		{
			if(!_authService.Verify(model.Code,model.UserName))return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var success = _usersService.Edit(model.UserName, (x) =>
			{
				x.AuthKey = model.NewKey;
			});
			if(!success)return new JsonResult(ActionStatusMessage.User.NotExist);
			_unitOfWork.Save();
			return new JsonResult(ActionStatusMessage.Success);
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("rest")]
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
		[Route("rest")]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var codeResult = _verifyService.Verify(model.Verify);
				if (!codeResult)
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
		[Route("rest")]
		public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new Status(ActionStatusMessage.Fail.status, JsonConvert.SerializeObject(ModelState.AllModelStateErrors())));
			if (!_verifyService.Verify(model.Verify))
			{
				return new JsonResult(ActionStatusMessage.Account.Auth.Verify.Invalid);
			}
			if (!_authService.Verify(model.Auth.Code, model.Auth.UserName)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);

			var user = await _usersService.CreateAsync(model.UserName, model.Email, model.Password, model.Company);
			if (user == null)
			{
				return new JsonResult(ActionStatusMessage.Account.Register.UserExist);
			}
			_logger.LogInformation($"新的用户创建:{model.UserName}");

			var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
			await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

			await _signInManager.SignInAsync(user, isPersistent: false);
			return new JsonResult(ActionStatusMessage.Success);
		}

		#region Permission
		
		[HttpGet]
		[Route("rest")]
		public IActionResult Permission(string username)
		{
			var currentUser = _authService.CurrentUserService.CurrentUser;
			username = username ?? currentUser.UserName;
			var targetUser = _usersService.Get(username);
			if(targetUser==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			if (currentUser.UserName == username || targetUser.Privilege < currentUser.Privilege)
			{
				return new JsonResult(new PermissionCompaniesQueryViewModel()
				{
					Data = new PermissionCompaniesQueryDataModel()
					{
						List = targetUser.PermissionCompanies
					}
				});
			}else return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
		}

		[HttpPost]
		[Route("rest")]
		public IActionResult Permission(string targetUser,string path,GoogleAuthViewModel authCode)
		{
			var authUser = _usersService.Get(authCode.UserName);
			targetUser = _authService.CurrentUserService.CurrentUser.UserName;
			if (authUser==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			var password = authUser.AuthKey??authUser.UserName;
			if(!_authService.Verify(authCode.Code, authCode.UserName, password))return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			bool authUserHavePermission = authUser.PermissionCompanies.Any(per=>path.StartsWith(per.Path));
			if(!authUserHavePermission)return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			
			bool success=_usersService.Edit(targetUser, user =>
			{
				if (user.PermissionCompanies.Any(per => path == per.Path)) return;
				var permissionCompany = new PermissionCompany()
				{
					AuthBy = authUser.Id,
					Path=path,
					Owner = _authService.CurrentUserService.CurrentUser
				};
				_unitOfWork.PermissionCompanies.Create(permissionCompany);
				user.PermissionCompanies.Append(permissionCompany);
			});
			if (!success) return new JsonResult(ActionStatusMessage.User.NotExist);
			_unitOfWork.Save();
			return new JsonResult(ActionStatusMessage.Success);
		}

		[HttpDelete]
		[Route("rest")]
		public ActionResult Permission(Guid id, GoogleAuthViewModel authCode)
		{
			var authUser = _usersService.Get(authCode.UserName);
			if (authUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var password = authUser.AuthKey ?? authUser.UserName;
			if (!_authService.Verify(authCode.Code, authCode.UserName, password)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var targetPermission = _unitOfWork.PermissionCompanies.Get(id);
			if(targetPermission==null)return new JsonResult(ActionStatusMessage.Account.Auth.Permission.NotExist);
			var targetUser = targetPermission.Owner;
			if (targetPermission.AuthBy == authUser.Id || targetUser.Privilege < authUser.Privilege)
			{
				var list = targetUser.PermissionCompanies.ToList();
				list.Remove(targetPermission);
				targetUser.PermissionCompanies=list;
				_unitOfWork.Users.Update(targetUser);
				_unitOfWork.Save();
				return new JsonResult(ActionStatusMessage.Success);
			}else return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
		}
		#endregion
		#endregion
		#region Helpers

		private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        private string EmailToUserName(string userName)
        {
            string result = "";
            foreach(char c in userName.ToCharArray())
            {
                if (c == '@')
                    break;

                result += c;
            }

            return result;
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
