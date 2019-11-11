using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.Linq;
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
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers
{
	/// <summary>
	/// 账号管理
	/// </summary>
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
		/// <summary>
		/// 账号管理
		/// </summary>
		/// <param name="userManager"></param>
		/// <param name="signInManager"></param>
		/// <param name="emailSender"></param>
		/// <param name="logger"></param>
		/// <param name="usersService"></param>
		/// <param name="verifyService"></param>
		/// <param name="authService"></param>
		/// <param name="context"></param>
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




		#region Rest
		/// <summary>
		/// 通过身份证号查询身份号
		/// </summary>
		/// <param name="cid">身份证号</param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(Status), 0)]
		public IActionResult GetUserIdByCid(string cid)
		{
			if(cid==null)return new JsonResult(ActionStatusMessage.User.NoId);
			if(cid.Length!=18)return new JsonResult(ActionStatusMessage.User.NotCorrectId);
			var user = _context.AppUsers.FirstOrDefault(u => u.BaseInfo.Cid == cid);
			if(user==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			return new JsonResult(new UserIdByCidViewModel()
			{
				Data = new UserIdByCidDataModel()
				{
					Id = user.Id
				}
			});
		}
		/// <summary>
		/// 确认邮箱
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="code">系统生成的校验码</param>
		/// <remarks>
		///用户注册后，用户的邮箱将收到账号激活邮件，点击后将返回此处
		/// </remarks>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(Status),0)]
		public async Task<IActionResult> ConfirmEmail(string userId, string code)
		{
			if (userId == null || code == null)
			{
				return new JsonResult(ActionStatusMessage.Fail);
			}
			var user = await _userManager.FindByIdAsync(userId) ??
			           throw new ApplicationException($"无法加载当前用户信息 '{userId}'.");
			var result = await _userManager.ConfirmEmailAsync(user, code);
			return new JsonResult(ActionStatusMessage.Success);
		}
		/// <summary>
		/// 退出登录
		/// </summary>
		/// <returns>需要登录</returns>
		[HttpPost]
		[ProducesResponseType(typeof(Status), 0)]

		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			_logger.LogInformation("User logged out.");

			return new JsonResult(ActionStatusMessage.Success);
		}
		/// <summary>
		/// 获取当前用户的权限
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(IDictionary<string, PermissionRegion>), 0)]

		public IActionResult Permission([FromBody]QueryPermissionsViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			if (model.Auth==null||!model.Auth.Verify(_authService))return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var targetUser = _usersService.Get(model.Id);
			if(targetUser==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			var permission = targetUser.Application.Permission;
			return new JsonResult(new QueryPermissionsOutViewModel(){Data =permission.GetRegionList() });
		}
		/// <summary>
		/// 修改权限情况
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(typeof(Status), 0)]

		public IActionResult Permission([FromBody]ModifyPermissionsViewModel model)
		{
			if (!_authService.Verify(model.Auth.Code, model.Auth.AuthByUserID)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var targetUser = _usersService.Get(model.Id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var authUser = _usersService.Get(model.Auth.AuthByUserID);
			if(authUser==null) return new JsonResult(ActionStatusMessage.User.NotExist);
			if (!targetUser.Application.Permission.Update(model.NewPermission, authUser.Application.Permission))return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			_usersService.Edit(targetUser);
			return new JsonResult(ActionStatusMessage.Success);
		}
		/// <summary>
		/// 修改安全码
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(Status), 0)]

		public IActionResult AuthKey([FromBody]ModifyAuthKeyViewModel model)
		{
			var targetUser = _usersService.Get(model.ModifyUserId);
			if(targetUser==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			if (!model.Auth.Verify(_authService))return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var authByUser = _usersService.Get(model.Auth.AuthByUserID);
			if(authByUser==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			if (!authByUser.Application.Permission.Check(DictionaryAllPermission.User.Application,Operation.Update,targetUser))return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			targetUser.Application.AuthKey = model.NewKey;
			var success = _usersService.Edit(targetUser);
			if(!success)return new JsonResult(ActionStatusMessage.User.NotExist);
			
			return new JsonResult(ActionStatusMessage.Success);
		}
		/// <summary>
		/// 获取安全码 二维码
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(Image), 0)]

		public IActionResult AuthKey()
		{
			if(!User.Identity.IsAuthenticated)return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			var qrCoder = new BLL.Helpers.SfQRCoder();
			_authService.InitCode();
			var img = qrCoder.GenerateBytes(_authService.Url);
			HttpContext.Response.Cookies.Append("key",_authService.Code().ToString());
			return new FileContentResult(img,"image/png");
		}
		/// <summary>
		/// 用户登录界面回调
		/// </summary>
		/// <param name="returnUrl">登录回调页面</param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(Status), 0)]

		public IActionResult Login(string returnUrl)
		{
			return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
		}
		/// <summary>
		/// 用户登录
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(Status), 0)]

		public async Task<IActionResult> Login([FromBody]LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var r = model.Verify.Verify(_verifyService);
				if (r != "") return new JsonResult(new Status(ActionStatusMessage.Account.Auth.Verify.Invalid.status, r));

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
		/// <summary>
		/// 移除用户
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpDelete]
		[AllowAnonymous]
		[ProducesResponseType(typeof(Status), 0)]

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
		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(Status),0)]
		public async Task<IActionResult> Application([FromBody]UserApplicationViewModel model)
		{
			if (!_authService.Verify(model.Auth.Code, model.Auth.AuthByUserID)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var authByUser = _usersService.Get(model.Auth.AuthByUserID);
			var targetUser = _usersService.Get(model.Data.Id);
			if (authByUser == null || targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			if (!authByUser.Application.Permission.Check(DictionaryAllPermission.User.Application, Operation.Update, targetUser)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			_context.Users.Update(model.Data);
			await _context.SaveChangesAsync();
			return new JsonResult(ActionStatusMessage.Success);
		}
		/// <summary>
		/// 注册新用户
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(Status), 0)]

		public async Task<IActionResult> Register([FromBody]UserCreateViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			var r = model.Verify.Verify(_verifyService);
			if (r!="") return new JsonResult(new Status(ActionStatusMessage.Account.Auth.Verify.Invalid.status,r));
			
			if (!_authService.Verify(model.Auth.Code, model.Auth.AuthByUserID)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var authByUser =  _usersService.Get(model.Auth.AuthByUserID);
			if(authByUser != null)return new JsonResult(ActionStatusMessage.Account.Register.UserExist);
			if (model.Data.Company == null) return new JsonResult(ActionStatusMessage.Company.NotExist);
			if (!authByUser.Application.Permission.Check(DictionaryAllPermission.User.Application, Operation.Update, model.Data.Company.Company)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var user = await _usersService.CreateAsync(model.Data.ToDTO(model.Auth.AuthByUserID,_context.AdminDivisions),model.Data.ConfirmPassword);
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
					Name = model.Data.Company.Duties.Name
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
