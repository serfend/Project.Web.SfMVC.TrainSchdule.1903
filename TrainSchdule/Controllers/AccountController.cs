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
using TrainSchdule.ViewModels.User;
using BLL.Extensions;
using TrainSchdule.ViewModels.System;

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
		private readonly ICurrentUserService currentUserService;
		private readonly ApplicationDbContext _context;
		private readonly IEmailSender _emailSender;
		private readonly ILogger _logger;
		private readonly IUsersService _usersService;
		private readonly IVerifyService _verifyService;
		private readonly IGoogleAuthService _authService;
		private readonly IUserActionServices _userActionServices;


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
		/// <param name="currentUserService"></param>
		/// <param name="userActionServices"></param>
		public AccountController(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IEmailSender emailSender,
			ILogger<AccountController> logger,
			IUsersService usersService, IVerifyService verifyService, IGoogleAuthService authService, ApplicationDbContext context, ICurrentUserService currentUserService, IUserActionServices userActionServices)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_emailSender = emailSender;
			_logger = logger;
			_usersService = usersService;
			_verifyService = verifyService;
			_authService = authService;
			_context = context;
			this.currentUserService = currentUserService;
			_userActionServices = userActionServices;
		}

		#endregion




		#region Rest
		/// <summary>
		/// 获取用户操作记录
		/// </summary>
		/// <param name="page"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(UserActionViewModel), 0)]

		public IActionResult UserAction(int page, int pageSize = 20)
		{
			var currentUser = currentUserService.CurrentUser;
			var list = _context.UserActions.Where(u => u.UserName == currentUser.Id);
			var count = list.Count();
			list = list.Skip(page * pageSize).Take(pageSize);
			return new JsonResult(new UserActionViewModel()
			{
				Data = new UserActionDataModel()
				{
					List = list,
					TotalCount = count
				}
			});
		}
		/// <summary>
		/// 通过用户真实姓名查询身份号
		/// </summary>
		/// <param name="realName"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(UserBaseInfoWithIdViewModel), 0)]
		public IActionResult GetUserIdByRealName(string realName)
		{
			if (realName == null) return new JsonResult(ActionStatusMessage.User.NoId);
			var users = _context.AppUsers.Where(u => u.BaseInfo.RealName == realName).ToList();
			if (users.Count == 0) users = _context.AppUsers.Where(u => u.BaseInfo.RealName.Contains(realName) > 0).ToList();
			return new JsonResult(new UsersBaseInfoWithIdViewModel()
			{
				Data = new UsersBaseInfoWithIdDataModel()
				{
					List = users.Select(u => new UserBaseInfoWithIdDataModel()
					{
						Base = u.BaseInfo,
						Id = u.Id
					})
				}
			});
		}
		/// <summary>
		/// 通过身份证号查询身份号
		/// </summary>
		/// <param name="cid">身份证号</param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(UserBaseInfoWithIdViewModel), 0)]
		public IActionResult GetUserIdByCid(string cid)
		{
			if (cid == null) return new JsonResult(ActionStatusMessage.User.NoId);
			if (cid.Length != 18) return new JsonResult(ActionStatusMessage.User.NotCorrectId);
			var user = _context.AppUsers.FirstOrDefault(u => u.BaseInfo.Cid == cid);
			if (user == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			return new JsonResult(new UserBaseInfoWithIdViewModel()
			{
				Data = new UserBaseInfoWithIdDataModel()
				{
					Base = user.BaseInfo,
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
		[ProducesResponseType(typeof(ApiResult), 0)]
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
		[ProducesResponseType(typeof(ApiResult), 0)]

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

		public IActionResult Permission(string id)
		{
			var currentId = id ?? currentUserService.CurrentUser.Id;
			if (currentId == null) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			var targetUser = _usersService.Get(currentId);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var permission = targetUser.Application.Permission;
			return new JsonResult(new QueryPermissionsOutViewModel() { Data = permission.GetRegionList() });
		}
		/// <summary>
		/// 修改权限情况
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(typeof(ApiResult), 0)]

		public IActionResult Permission([FromBody]ModefyPermissionsViewModel model)
		{
			if (!model.Auth.Verify(_authService)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var targetUser = _usersService.Get(model.Id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var authUser = _usersService.Get(model.Auth.AuthByUserID);
			if (authUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			if (!targetUser.Application.Permission.Update(model.NewPermission, authUser.Application.Permission)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			_usersService.Edit(targetUser);
			return new JsonResult(ActionStatusMessage.Success);
		}
		/// <summary>
		/// 修改密码
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(typeof(ApiResult), 0)]

		public async Task<IActionResult> Password([FromBody]ModefyPasswordViewModel model)
		{
			var currentUser = currentUserService.CurrentUser;
			var cid = model.Id;
			if (model.Id.Length == 18) model.Id = _context.AppUsers.Where(u => u.BaseInfo.Cid == cid).FirstOrDefault()?.Id;
			var targetUser = _usersService.Get(model?.Id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			if (model.Id != currentUser?.Id && _userActionServices.Permission(currentUser.Application.Permission, DictionaryAllPermission.User.Application, Operation.Update, currentUser.Id, targetUser.CompanyInfo.Company.Code) == false) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			if (!ModelState.IsValid) return new JsonResult(ModelState.ToModel());
			var appUser = _context.Users.Where(u => u.UserName == model.Id).FirstOrDefault();

			model.ConfirmNewPassword = _usersService.ConvertFromUserCiper(cid, model.ConfirmNewPassword);
			model.NewPassword = _usersService.ConvertFromUserCiper(cid, model.NewPassword);
			if (model.NewPassword != model.ConfirmNewPassword) return new JsonResult(ActionStatusMessage.Account.Register.ConfirmPasswordNotSame);
			model.OldPassword = _usersService.ConvertFromUserCiper(cid, model.OldPassword);
			if (model.OldPassword == null || model.NewPassword == null || model.ConfirmNewPassword == null) return new JsonResult(ActionStatusMessage.Account.Login.ByUnknown);

			var sign = await _signInManager.PasswordSignInAsync(appUser, model.OldPassword, false, false);
			if (!sign.Succeeded) return new JsonResult(ActionStatusMessage.Account.Login.AuthAccountOrPsw);
			if (model.ConfirmNewPassword == model.OldPassword) return new JsonResult(ActionStatusMessage.Account.Login.PasswordIsSame);
			appUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(appUser, model.ConfirmNewPassword);
			_context.Users.Update(appUser);
			targetUser.BaseInfo.PasswordModefy = true;
			_context.AppUserBaseInfos.Update(targetUser.BaseInfo);
			await _context.SaveChangesAsync();
			return new JsonResult(ActionStatusMessage.Success);
		}
		/// <summary>
		/// 修改安全码
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResult), 0)]

		public IActionResult AuthKey([FromBody]ModifyAuthKeyViewModel model)
		{
			var targetUser = _usersService.Get(model.ModifyUserId);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			if (!model.Auth.Verify(_authService)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var authByUser = _usersService.Get(model.Auth.AuthByUserID);
			if (authByUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			if (!_userActionServices.Permission(authByUser.Application.Permission, DictionaryAllPermission.User.Application, Operation.Update, authByUser.Id, targetUser.CompanyInfo.Company.Code)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			targetUser.Application.AuthKey = model.NewKey;
			var success = _usersService.Edit(targetUser);
			if (!success) return new JsonResult(ActionStatusMessage.User.NotExist);

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
			if (!User.Identity.IsAuthenticated) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			var qrCoder = new BLL.Helpers.SfQRCoder();
			_authService.InitCode();
			var img = qrCoder.GenerateBytes(_authService.Url);
			HttpContext.Response.Cookies.Append("key", _authService.Code().ToString());
			return new FileContentResult(img, "image/png");
		}
		/// <summary>
		/// 用户登录界面回调
		/// </summary>
		/// <param name="returnUrl">登录回调页面</param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResult), 0)]

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
		[ProducesResponseType(typeof(ApiResult), 0)]

		public async Task<IActionResult> Login([FromBody]LoginViewModel model)
		{
			var actionRecord = _userActionServices.Log(UserOperation.Login, model?.UserName, "");
			if (ModelState.IsValid)
			{
				var r = model.Verify.Verify(_verifyService);
				if (r != "") return new JsonResult(new ApiResult(ActionStatusMessage.Account.Auth.Verify.Invalid.Status, r));
				var cid = model.UserName;
				if (model.UserName.Length == 18) model.UserName = _context.AppUsers.Where(u => u.BaseInfo.Cid == cid).FirstOrDefault()?.Id;
				var targetUser = _usersService.Get(model.UserName);
				if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
				model.Password = _usersService.ConvertFromUserCiper(cid, model.Password);
				if (model.Password == null) return new JsonResult(ActionStatusMessage.Account.Login.AuthAccountOrPsw);
				var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
				if (result.Succeeded)
				{
					_logger.LogInformation($"用户登录:{model.UserName}");
					_userActionServices.Status(actionRecord, true);
					return new JsonResult(ActionStatusMessage.Success);
				}
				else if (result.RequiresTwoFactor)
				{
					_userActionServices.Status(actionRecord, false, "账号需要二次验证");
					return new JsonResult(ActionStatusMessage.Account.Login.AuthException);
				}
				else if (result.IsLockedOut)
				{
					_logger.LogWarning("账号异常");
					_userActionServices.Status(actionRecord, false, "账号已处于锁定状态");

					return new JsonResult(ActionStatusMessage.Account.Login.AuthBlock);
				}
				else return new JsonResult(ActionStatusMessage.Account.Login.AuthAccountOrPsw);
			}
			else return new JsonResult(new ApiResult(ActionStatusMessage.Fail.Status, JsonConvert.SerializeObject(ModelState.AllModelStateErrors())));
		}
		/// <summary>
		/// 批量移除用户
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpDelete]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResult), 0)]
		public async Task<IActionResult> RemoveMutil([FromBody]UserRemoveMutiViewMode model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			if (!model.Auth.Verify(_authService)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var authByUser = _usersService.Get(model.Auth.AuthByUserID);
			if (authByUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var statusME = new Dictionary<string, ApiResult>();

			foreach (var u in model.Data?.Id)
				try
				{
					await RemoveSingle(u, authByUser);
				}
				catch (ActionStatusMessageException ex)
				{
					statusME.Add(u, ex.Status);
				}
				finally { }

			return new JsonResult(new ResponseStatusOrModelExceptionViweModel(statusME.Count > 0 ? ActionStatusMessage.Fail : ActionStatusMessage.Success)
			{
				StatusException = statusME
			});
		}
		/// <summary>
		/// 移除用户
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpDelete]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResult), 0)]

		public async Task<IActionResult> Remove([FromBody] UserRemoveViewModel model)
		{
			if (!model.Auth.Verify(_authService)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			try
			{
				var authByUser = _usersService.Get(model.Auth.AuthByUserID);
				if (authByUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
				await RemoveSingle(model.Id, authByUser);
			}
			catch (ActionStatusMessageException ex)
			{
				return new JsonResult(ex.Status);
			}
			return new JsonResult(ActionStatusMessage.Success);
		}

		private async Task RemoveSingle(string id, User authByUser)
		{
			var actionRecord = _userActionServices.Log(UserOperation.Remove, id, "");
			var targetUser = _usersService.Get(id);
			if (targetUser == null) throw new ActionStatusMessageException(ActionStatusMessage.User.NotExist);
			if (!_userActionServices.Permission(authByUser.Application.Permission, DictionaryAllPermission.User.Application, Operation.Update, authByUser.Id, targetUser.CompanyInfo?.Company?.Code)) throw new ActionStatusMessageException(ActionStatusMessage.Account.Auth.Invalid.Default);
			if (!await _usersService.RemoveAsync(id)) throw new ActionStatusMessageException(ActionStatusMessage.User.NotExist);
			_userActionServices.Status(actionRecord, true);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResult), 0)]
		public async Task<IActionResult> Application([FromBody]UserApplicationViewModel model)
		{
			if (!model.Auth.Verify(_authService)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var authByUser = _usersService.Get(model.Auth.AuthByUserID);
			var targetUser = _usersService.Get(model.Id);
			if (authByUser == null || targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			if (!authByUser.Application.Permission.Check(DictionaryAllPermission.User.Application, Operation.Update, targetUser.CompanyInfo.Company.Code)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var app = targetUser.Application;
			app.Email = model.Data.Email;
			app.AuthKey = model.Data.AuthKey;
			app.ApplicationSetting.LastSubmitApplyTime = model.Data.ApplicationSetting.LastSubmitApplyTime;
			_context.AppUserApplicationInfos.Update(app);
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
		[ProducesResponseType(typeof(ApiResult), 0)]

		public async Task<IActionResult> Register([FromBody]UserCreateViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			var r = model.Verify.Verify(_verifyService);
			if (r != "") return new JsonResult(new ApiResult(ActionStatusMessage.Account.Auth.Verify.Invalid.Status, r));

			if (!model.Auth.Verify(_authService)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var authByUser = _usersService.Get(model.Auth.AuthByUserID);
			try
			{
				await RegisterSingle(model.Data, authByUser, $"{authByUser.Id}常规注册");
			}
			catch (ActionStatusMessageException ex)
			{
				return new JsonResult(ex.Status);
			}
			catch (ModelStateException mse)
			{
				return new JsonResult(mse.Model);
			}
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 注册新用户
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResult), 0)]

		public async Task<IActionResult> RegisterMutil([FromBody]UsersCreateMutilViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			var r = model.Verify.Verify(_verifyService);
			if (r != "") return new JsonResult(new ApiResult(ActionStatusMessage.Account.Auth.Verify.Invalid.Status, r));

			if (!model.Auth.Verify(_authService)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var authByUser = _usersService.Get(model.Auth.AuthByUserID);
			var exStatus = new Dictionary<string, ApiResult>();
			var exMSE = new Dictionary<string, ModelStateExceptionDataModel>();
			foreach (var m in model.Data.List)
				try
				{
					await RegisterSingle(m, authByUser, $"{authByUser.Id}批量注册");
				}
				catch (ActionStatusMessageException ex)
				{
					exStatus.Add(m.Application?.UserName, ex.Status);
				}
				catch (ModelStateException mse)
				{
					exMSE.Add(m.Application?.UserName, mse.Model.Data);
					ModelState.Clear();
				}
				finally { }
			return new JsonResult(new ResponseStatusOrModelExceptionViweModel(exMSE.Count > 0 || exStatus.Count > 0 ? ActionStatusMessage.Fail : ActionStatusMessage.Success)
			{
				ModelStateException = exMSE,
				StatusException = exStatus
			});
		}
		#endregion

		private async Task RegisterSingle(UserCreateDataModel model, User authByUser, string regDescription)
		{
			if (model.Application?.UserName == null) throw new ActionStatusMessageException(ActionStatusMessage.User.NoId);
			var regUser = _usersService.Get(model.Application.UserName);
			var actionRecord = _userActionServices.Log(UserOperation.Register, model.Application.UserName, "");
			if (regUser != null) throw new ActionStatusMessageException(ActionStatusMessage.Account.Register.UserExist);
			if (model.Company == null) throw new ActionStatusMessageException(ActionStatusMessage.Company.NotExist);
			if (!_userActionServices.Permission(authByUser.Application.Permission, DictionaryAllPermission.User.Application, Operation.Update, authByUser.Id, model.Company.Company.Code)) throw new ActionStatusMessageException(ActionStatusMessage.Account.Auth.Invalid.Default);
			var user = await _usersService.CreateAsync(model.ToDTO(authByUser.Id, _context.AdminDivisions), model.ConfirmPassword);
			if (user == null) throw new ActionStatusMessageException(ActionStatusMessage.Account.Register.Default);

			var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
			await _emailSender.SendEmailConfirmationAsync(user.Email, callbackUrl);
			var currentUser = _usersService.Get(user.UserName);
			if (currentUser.CompanyInfo.Company == null) ModelState.AddModelError("company", "单位不存在");
			if (currentUser.CompanyInfo.Duties == null) ModelState.AddModelError("duties", "职务不存在");
			var anyCodeInvalid = currentUser.SocialInfo.Settle.AnyCodeInvalid();
			if (anyCodeInvalid != null) ModelState.AddModelError("settle", $"无效的行政区划:{anyCodeInvalid}");

			if (!ModelState.IsValid)
			{
				await Remove(new UserRemoveViewModel()
				{
					Auth = GoogleAuthDataModel.Root,
					Id = model.Application.UserName
				});
				throw new ModelStateException(new ModelStateExceptionViewModel(ModelState));
			}   //await _signInManager.SignInAsync(user, isPersistent: false);
			_logger.LogInformation($"新的用户创建:{user.UserName}");
			_userActionServices.Status(actionRecord, true, regDescription);
		}
	}
}
