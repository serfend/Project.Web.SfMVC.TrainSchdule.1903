using System;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.Account;
using TrainSchdule.ViewModels;

namespace TrainSchdule.Controllers
{
	[Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        #region Fields

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IUsersService _usersService;
        private readonly IVerifyService _verifyService;
        private readonly IGoogleAuthService _authService;


        #endregion

        #region .ctors

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger,
            IUsersService usersService, IVerifyService verifyService, IGoogleAuthService authService, ApplicationDbContext context) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _usersService = usersService;
            _verifyService = verifyService;
            _authService = authService;
            _context = context;
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

		[HttpGet]
		public IActionResult Permission([FromBody]QueryPermissionsViewModel model)
		{
			if(!_authService.Verify(model.Auth.Code,model.Auth.AuthByUserID))return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var targetUser = _usersService.Get(model.id);
			if(targetUser==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			var permission = targetUser.Application.Permission;
			return new JsonResult(new QueryPermissionsOutViewModel(){Data =permission.GetRegionList() });
		}
		[HttpPost]
		public IActionResult Permission([FromBody]ModifyPermissionsViewModel model)
		{
			if (!_authService.Verify(model.Auth.Code, model.Auth.AuthByUserID)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var targetUser = _usersService.Get(model.id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var authUser = _usersService.Get(model.Auth.AuthByUserID);
			if(authUser==null) return new JsonResult(ActionStatusMessage.User.NotExist);
			if (!targetUser.Application.Permission.Update(model.NewPermission, authUser.Application.Permission))return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			_usersService.Edit(targetUser);
			return new JsonResult(ActionStatusMessage.Success);
		}
		[HttpPost]
		[AllowAnonymous]
		public IActionResult AuthKey([FromBody]ModifyAuthKeyViewModel model)
		{
			var targetUser = _usersService.Get(model.ModifyUserId);
			if(targetUser==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			if (!_authService.Verify(model.Auth.Code,model.Auth.AuthByUserID))return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var authByUser = _usersService.Get(model.Auth.AuthByUserID);
			if(authByUser==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			if (!authByUser.Application.Permission.Check(DictionaryAllPermission.User.Application,Operation.Update,targetUser))return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			targetUser.Application.AuthKey = model.NewKey;
			var success = _usersService.Edit(targetUser);
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

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Login(string returnUrl)
		{
			return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
		}
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromBody]LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				if (!model.Verify.Verify(_verifyService))
				{
					return new JsonResult(ActionStatusMessage.Account.Auth.Verify.Invalid);
				}

				var targetUser = _usersService.Get(model.UserName);
				if(targetUser==null)return new JsonResult(ActionStatusMessage.User.NotExist);
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

		[HttpDelete]
		[AllowAnonymous]
		public async Task<IActionResult> Remove([FromBody] UserRemoveViewModel model)
		{
			if (!_authService.Verify(model.Auth.Code, model.Auth.AuthByUserID)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var authByUser = _usersService.Get(model.Auth.AuthByUserID);
			if(authByUser==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			var targetUser = _usersService.Get(model.Id);
			if(targetUser==null) return new JsonResult(ActionStatusMessage.User.NotExist);
			if (!authByUser.Application.Permission.Check(DictionaryAllPermission.User.Application, Operation.Update,targetUser)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			if (!await _usersService.RemoveAsync(model.Id))return new JsonResult(ActionStatusMessage.User.NotExist);
			return new JsonResult(ActionStatusMessage.Success);
		}
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Register([FromBody]UserCreateViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			if (!model.Verify.Verify(_verifyService)) return new JsonResult(ActionStatusMessage.Account.Auth.Verify.Invalid);
			
			if (!_authService.Verify(model.Auth.Code, model.Auth.AuthByUserID)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var checkUser =  _usersService.Get(model.Data.Id);
			if(checkUser!=null)return new JsonResult(ActionStatusMessage.Account.Register.UserExist);
			var user = await _usersService.CreateAsync(model.Data.ToDTO(model.Auth.AuthByUserID));
			if (user == null)
			{
				return new JsonResult(ActionStatusMessage.Account.Register.UserExist);
			}

			_logger.LogInformation($"新的用户创建:{user.UserName}");

			var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
			await _emailSender.SendEmailConfirmationAsync(user.Email, callbackUrl);
			var currentUser = _usersService.Get(user.UserName);
			if (currentUser.CompanyInfo.Company == null) ModelState.AddModelError("company","单位不存在");
			if (currentUser.CompanyInfo.Duties == null)
			{
				await _context.Duties.AddAsync(new Duties()
				{
					Name = model.Data.Duties
				});
				ModelState.AddModelError("duties", "职务不存在");
			}
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			//await _signInManager.SignInAsync(user, isPersistent: false);
			return new JsonResult(ActionStatusMessage.Success);
		}


        #endregion


    }
}
