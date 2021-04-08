﻿using System;
using System.Collections.Generic;
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
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Drawing.Imaging;
using DAL.DTO.User;
using BLL.Extensions.Common;
using static BLL.Extensions.UserExtensions;
using BLL.Interfaces.Common;
using Abp.Extensions;
using BLL.Interfaces.Permission;
using DAL.Entities.Permisstions;

namespace TrainSchdule.Controllers
{
	/// <summary>
	/// 账号管理
	/// </summary>
	[Authorize]
	[Route("[controller]/[action]")]
	public partial class AccountController : Controller
	{
		#region Fields

		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ICurrentUserService currentUserService;
		private readonly ApplicationDbContext context;
		private readonly IEmailSender _emailSender;
		private readonly ILogger _logger;
		private readonly IUsersService _usersService;
		private readonly IVerifyService _verifyService;
		private readonly IGoogleAuthService _authService;
		private readonly IUserActionServices _userActionServices;
		private readonly ICipperServices cipperServices;

		#endregion Fields

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
		/// <param name="cipperServices"></param>

		public AccountController(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IEmailSender emailSender,
			ILogger<AccountController> logger,
			IUsersService usersService, IVerifyService verifyService, IGoogleAuthService authService, ApplicationDbContext context, ICurrentUserService currentUserService, IUserActionServices userActionServices,
			ICipperServices cipperServices)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_emailSender = emailSender;
			_logger = logger;
			_usersService = usersService;
			_verifyService = verifyService;
			_authService = authService;
			this.context = context;
			this.currentUserService = currentUserService;
			_userActionServices = userActionServices;
			this.cipperServices = cipperServices;
		}

		#endregion .ctors

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
			var list = context.UserActionsDb.Where(u => u.UserName == currentUser.Id);
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
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(UserBaseInfoWithIdViewModel), 0)]
		public IActionResult GetUserIdByRealName(string realName, int pageIndex = 0, int pageSize = 20)
		{
			var users = _usersService.GetUserByRealname(realName);
			var result = users.SplitPage(pageIndex, pageSize);
			var r = result.Item1.ToList();
			var list = r.Select(u => u.ToSummaryDto());
			return new JsonResult(new EntitiesListViewModel<UserSummaryDto>(list, result.Item2));
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
			if (cid == null) return new JsonResult(ActionStatusMessage.UserMessage.NoId);
			if (!cid.CheckIDCard(out var reason)) return new JsonResult(new ApiResult(ActionStatusMessage.UserMessage.NotCrrectCid,reason,true));
			var user = context.AppUsersDb.FirstOrDefault(u => u.BaseInfo.Cid == cid);
			if (user == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
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
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
				return new JsonResult($"无法加载当前用户信息 '{userId}'.");
			await _userManager.ConfirmEmailAsync(user, code);
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
			if (currentUserService.CurrentUser?.Id == null) return new JsonResult(new ActionStatusMessageException(ActionStatusMessage.Account.Auth.Invalid.NotLogin));
			_userActionServices.Log(UserOperation.Logout, currentUserService.CurrentUser?.Id, "退出登录", true);
			await _signInManager.SignOutAsync();
			_logger.LogInformation("User logged out.");

			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 修改密码
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResult), 0)]
		public async Task<IActionResult> Password([FromBody] ModifyPasswordViewModel model)
		{
			string userid = null;
			var isCid = model.Id.CheckIDCard(out var _);
			// 身份证转id
			if (isCid) userid = context.AppUsersDb.Where(u => u.BaseInfo.Cid == model.Id).FirstOrDefault()?.Id;
			else userid = model.Id;
			// 目标用户权限判断
			var currentUser = currentUserService.CurrentUser;
			var targetUser = _usersService.GetById(userid);
			var ua = _userActionServices.Log(UserOperation.ModifyPsw, userid, $"通过{currentUser?.Id}");
			if (targetUser == null) return new JsonResult(_userActionServices.LogNewActionInfo(ua, ActionStatusMessage.UserMessage.NotExist));
			var authUser = currentUser;
			bool authUserPermission = false;
			if (model.Auth?.AuthByUserID != null)
			{
				authUser = _usersService.GetById(model.Auth.AuthByUserID);
				_userActionServices.Status(ua, false, $"授权自{model.Auth.AuthByUserID}");
				if (authUser == null) return new JsonResult(_userActionServices.LogNewActionInfo(ua, ActionStatusMessage.UserMessage.NotExist));
				if (!model.Auth.Verify(_authService, currentUser?.Id)) return new JsonResult(_userActionServices.LogNewActionInfo(ua, ActionStatusMessage.Account.Auth.AuthCode.Invalid));
				// 允许本人修改本人密码，允许本级以上修改密码
				authUserPermission = authUser.Id == userid;
			}
			if (authUser == null) return new JsonResult(_userActionServices.LogNewActionInfo(ua, ActionStatusMessage.Account.Auth.Invalid.NotLogin));
			if (!authUserPermission) authUserPermission = _userActionServices.Permission(authUser, ApplicationPermissions.User.Application.Password.Item, PermissionType.Write, targetUser.CompanyInfo.CompanyCode, $"修改{targetUser.Id}密码");
			if (!authUserPermission) return new JsonResult(_userActionServices.LogNewActionInfo(ua, ActionStatusMessage.Account.Auth.Invalid.Default));

			// 密码修改判断
			var appUser = context.Users.Where(u => u.UserName == targetUser.Id).FirstOrDefault();

			model.ConfirmNewPassword = model.ConfirmNewPassword.FromCipperToString(model.Id, cipperServices);
			model.NewPassword = model.NewPassword.FromCipperToString(model.Id, cipperServices);
			if (model.NewPassword == null || model.ConfirmNewPassword == null) return new JsonResult(_userActionServices.LogNewActionInfo(ua, ActionStatusMessage.Account.Login.AuthFormat));
			if (model.NewPassword.Length < 8) return new JsonResult(_userActionServices.LogNewActionInfo(ua, ActionStatusMessage.Account.Register.PasswordTooSimple));
			if (model.NewPassword != model.ConfirmNewPassword) return new JsonResult(_userActionServices.LogNewActionInfo(ua, ActionStatusMessage.Account.Register.ConfirmPasswordNotSame));
			model.OldPassword = model.OldPassword.FromCipperToString(model.Id, cipperServices);
			// 本人修改密码，则判断旧密码
			if (userid == authUser.Id)
			{
				var sign = await _signInManager.PasswordSignInAsync(appUser, model.OldPassword, false, false);
				if (!sign.Succeeded && !authUserPermission) return new JsonResult(_userActionServices.LogNewActionInfo(ua, ActionStatusMessage.Account.Login.AuthAccountOrPsw));
				if (model.ConfirmNewPassword == model.OldPassword) return new JsonResult(_userActionServices.LogNewActionInfo(ua, ActionStatusMessage.Account.Login.PasswordIsSame));
			}
			appUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(appUser, model.ConfirmNewPassword);
			context.Users.Update(appUser);
			targetUser.BaseInfo.PasswordModify = true;
			context.AppUserBaseInfos.Update(targetUser.BaseInfo);
			await context.SaveChangesAsync().ConfigureAwait(false);
			_userActionServices.Status(ua, true);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 检查授权码是否正确
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResult), 0)]
		public IActionResult CheckAuthCode([FromBody] GoogleAuthViewModel model)
		{
			var r = model?.Auth?.Verify(_authService, currentUserService.CurrentUser?.Id);
			if (!r.HasValue) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Default);
			if (r.Value) return new JsonResult(ActionStatusMessage.Success);
			return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
		}

		/// <summary>
		/// 修改安全码
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResult), 0)]
		public IActionResult AuthKey([FromBody] ModifyAuthKeyViewModel model)
		{
			var targetUser = _usersService.GetById(model.ModifyUserId);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
			if (!model.Auth.Verify(_authService, currentUserService.CurrentUser?.Id)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var authByUser = _usersService.GetById(model.Auth.AuthByUserID);
			if (authByUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
			if (!_userActionServices.Permission(authByUser, ApplicationPermissions.User.Application.AuthKey.Item, PermissionType.Write, targetUser.CompanyInfo.CompanyCode, $"修改{targetUser.Id}授权码")) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			targetUser.Application.AuthKey = model.NewKey;
			var success = _usersService.Edit(targetUser);
			if (!success) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);

			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 获取安全码 二维码
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(Image), 0)]
		public IActionResult AuthKey()
		{
			var realName = currentUserService.CurrentUser?.BaseInfo?.RealName;
			if (realName == null || realName.Length == 0) realName = null;
			var url = _authService.Init(currentUserService.CurrentUser?.Id)?.Main?.QrCodeUrl(realName);
			return new JsonResult(new { data = new { url }, status = 0 });
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
			Console.WriteLine(returnUrl);
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
		public async Task<IActionResult> Login([FromBody] LoginViewModel model)
		{
			string userid = null;
			bool isCid = model.UserName.CheckIDCard(out var _);
			if (isCid) userid = context.AppUsersDb.Where(u => u.BaseInfo.Cid == model.UserName).FirstOrDefault()?.Id;
			else userid = model.UserName;
			if (userid == null)
            {
				isCid = false;
				userid = model.UserName;
			}
			string loginType = isCid ? "身份证登录" : "用户登录";
			var actionRecord = _userActionServices.Log(UserOperation.Login, userid, $"{loginType}", false, ActionRank.Infomation);
			model.Verify.Verify(_verifyService);
			var targetUser = _usersService.GetById(userid);
			if (targetUser == null) {
				var check_user_exist = context.AppUsers.FirstOrDefault(u=>u.Id==userid);
				if(check_user_exist!=null)
					return new JsonResult(_userActionServices.LogNewActionInfo(actionRecord, (ActionStatusMessage.Account.Login.AccountRemoved)));
				else
					return new JsonResult(_userActionServices.LogNewActionInfo(actionRecord, (ActionStatusMessage.UserMessage.NotExist)));
			}
			var accountType = targetUser.Application.InvalidAccount();
			if (accountType == AccountType.NotBeenAuth)
				return new JsonResult(_userActionServices.LogNewActionInfo(actionRecord, ActionStatusMessage.Account.Auth.Permission.SystemInvalid));
			if (accountType == AccountType.Deny) return new JsonResult(_userActionServices.LogNewActionInfo(actionRecord, ActionStatusMessage.Account.Auth.Permission.SystemAllReadyInvalid));
			model.Password = model.Password.FromCipperToString(model.UserName, cipperServices);
			if (model.Password == null) return new JsonResult(_userActionServices.LogNewActionInfo(actionRecord, ActionStatusMessage.Account.Login.AuthAccountOrPsw));
			if (((int)targetUser.AccountStatus & (int)AccountStatus.Banned) > 0)
				return new JsonResult(_userActionServices.LogNewActionInfo(actionRecord, new ApiResult(ActionStatusMessage.Account.Login.BeenBanned, $"于{targetUser.StatusBeginDate}被封禁，{targetUser.StatusEndDate}解除", true)));
			// it seems if use persistent , cookie cant save expectly
			var result = await _signInManager.PasswordSignInAsync(userid, model.Password, false, lockoutOnFailure: false);
			if (result.Succeeded)
			{
				_logger.LogInformation($"用户登录:{userid}");
				_userActionServices.Status(actionRecord, true);
				return new JsonResult(ActionStatusMessage.Success);
			}
			else if (result.RequiresTwoFactor)
				return new JsonResult(_userActionServices.LogNewActionInfo(actionRecord, ActionStatusMessage.Account.Login.AuthException));
			else if (result.IsLockedOut)
				return new JsonResult(_userActionServices.LogNewActionInfo(actionRecord, ActionStatusMessage.Account.Login.AuthBlock));
			else
				return new JsonResult(_userActionServices.LogNewActionInfo(actionRecord, ActionStatusMessage.Account.Login.AuthAccountOrPsw));
		}

		/// <summary>
		/// 批量移除用户
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpDelete]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResult), 0)]
		public async Task<IActionResult> RemoveMutil([FromBody] UsersRemoveViewModel model)
		{
			if (!model.Auth.Verify(_authService, currentUserService.CurrentUser?.Id)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var authByUser = _usersService.GetById(model.Auth.AuthByUserID);
			if (authByUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
			var statusME = new Dictionary<string, ApiResult>();
			var id_str = string.Join("##", model.Data?.Select(i=>i.Id));
			var ua = _userActionServices.Log(UserOperation.Remove, authByUser?.Id, $"批量移除账号 {id_str}");
			foreach (var u in model.Data)
				try
				{
					await RemoveSingle(u, authByUser);
				}
				catch (ActionStatusMessageException ex)
				{
					statusME.Add(u?.Id, ex.Status);
				}
				finally { }
			if (statusME.Count == 0)
				_userActionServices.Status(ua, true);
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
			if (!model.Auth.Verify(_authService, currentUserService.CurrentUser?.Id)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var ua = _userActionServices.Log(UserOperation.Remove, currentUserService.CurrentUser?.Id, $"常规移除账号 {model?.Data?.Id}");
			try
			{
				var authByUser = _usersService.GetById(model.Auth.AuthByUserID);
				if (authByUser == null) return new JsonResult(_userActionServices.LogNewActionInfo(ua, ActionStatusMessage.UserMessage.NotExist));
				await RemoveSingle(model.Data, authByUser);
			}
			catch (ActionStatusMessageException ex)
			{
				return new JsonResult(_userActionServices.LogNewActionInfo(ua, ex.Status));
			}
			_userActionServices.Status(ua, true);
			return new JsonResult(ActionStatusMessage.Success);
		}
		/// <summary>
		/// 恢复账号
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPut]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResult), 0)]
		public async Task<IActionResult> Restore([FromBody] UserRemoveViewModel model)
		{
			if (!model.Auth.Verify(_authService, currentUserService.CurrentUser?.Id)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var ua = _userActionServices.Log(UserOperation.Restore, currentUserService.CurrentUser?.Id, $"恢复账号 {model?.Data?.Id}");
			try
			{
				var authByUser = _usersService.GetById(model.Auth.AuthByUserID);
				if (authByUser == null) return new JsonResult(_userActionServices.LogNewActionInfo(ua, ActionStatusMessage.UserMessage.NotExist));
				await RemoveSingle(model.Data, authByUser,false);
			}
			catch (ActionStatusMessageException ex)
			{
				return new JsonResult(_userActionServices.LogNewActionInfo(ua, ex.Status));
			}
			_userActionServices.Status(ua, true);
			return new JsonResult(ActionStatusMessage.Success);
		}

		private async Task RemoveSingle(UserRemoveDataModel u, User authByUser,bool isRemove = true)
		{
			var actionRecord = _userActionServices.Log(isRemove?UserOperation.Remove:UserOperation.Restore, u.Id, $"因：${u.Reason}", false, ActionRank.Danger);
			var targetUser = isRemove? _usersService.GetById(u.Id):context.AppUsers.FirstOrDefault(i=>i.Id==u.Id);
			if (targetUser == null) throw new ActionStatusMessageException(ActionStatusMessage.UserMessage.NotExist);
			if (!_userActionServices.Permission(authByUser, ApplicationPermissions.User.Application.Item,PermissionType.Write, targetUser.CompanyInfo?.CompanyCode, $"移除{targetUser.Id}账号")) throw new ActionStatusMessageException(_userActionServices.LogNewActionInfo(actionRecord, ActionStatusMessage.Account.Auth.Invalid.Default));
			var result = isRemove ? await _usersService.RemoveAsync(u.Id, u.Reason) :  _usersService.RestoreUser(u.Id);
			if (!result) throw new ActionStatusMessageException(_userActionServices.LogNewActionInfo(actionRecord, ActionStatusMessage.UserMessage.NotExist));
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
		public async Task<IActionResult> Application([FromBody] UserApplicationViewModel model)
		{
			if (!model.Auth.Verify(_authService, currentUserService.CurrentUser?.Id)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var authByUser = _usersService.GetById(model.Auth.AuthByUserID);
			var targetUser = _usersService.GetById(model.Id);
			if (authByUser == null || targetUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
			var ua = _userActionServices.Log(UserOperation.Permission, authByUser.Id, $"修改{targetUser.Id}系统信息");
			if (_userActionServices.Permission(authByUser, ApplicationPermissions.User.Application.Item,PermissionType.Write, targetUser.CompanyInfo.CompanyCode, $"修改{targetUser.Id}系统信息")) return new JsonResult(_userActionServices.LogNewActionInfo(ua, ActionStatusMessage.Account.Auth.Invalid.Default));
			var app = targetUser.Application;
			app.Email = model.Data.Email;
			app.AuthKey = model.Data.AuthKey;
			app.ApplicationSetting.LastSubmitApplyTime = model.Data.ApplicationSetting.LastSubmitApplyTime;
			context.AppUserApplicationInfos.Update(app);
			await context.SaveChangesAsync();
			_userActionServices.Status(ua, true);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 修改用户信息
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResult), 0)]
		public async Task<IActionResult> ModifyUser([FromBody] UserModifyViewModel model)
		{
			model.Verify?.Verify(_verifyService);
			var authByUser = currentUserService.CurrentUser ?? new User() { Id = null }; // 注册不需要使用授权，但邀请人为invalid
			if (model.Auth?.AuthByUserID != null)
			{
				if (!model.Auth.Verify(_authService, currentUserService.CurrentUser?.Id)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
				authByUser = _usersService.GetById(model.Auth.AuthByUserID);
			}
			var ua = _userActionServices.Log(UserOperation.ModifyUser, model.Data.Application.UserName, $"通过{authByUser.Id}修改用户信息", false, ActionRank.Danger);
			try
			{
				await ModifySingleUser(model.Data, authByUser);
			}
			catch (ActionStatusMessageException ex)
			{
				return new JsonResult(_userActionServices.LogNewActionInfo(ua, ex.Status));
			}
			_userActionServices.Status(ua, true);
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
		public async Task<IActionResult> Register([FromBody] UserCreateViewModel model)
		{
			model.Verify?.Verify(_verifyService);
			var authByUser = new User() { Id = null }; // 注册不需要使用授权，但邀请人为invalid
			if (model.Auth?.AuthByUserID != null)
			{
				if (!model.Auth.Verify(_authService, currentUserService.CurrentUser?.Id)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
				authByUser = _usersService.GetById(model.Auth.AuthByUserID);
			}
			var ua = _userActionServices.Log(UserOperation.Register, model?.Data?.Application?.UserName ?? "NOT Specify", $"{authByUser.Id}常规注册", false, ActionRank.Warning);
			await RegisterSingle(model.Data, authByUser);
			_userActionServices.Status(ua, true);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 对用户进行登录授权
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(typeof(ApiResult), 0)]
		public IActionResult AuthUserRegister([FromBody] AuthUserRegisterDataModel model)
		{
			var targetUser = _usersService.GetById(model.UserName);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
			if (targetUser.Application.InvitedBy != null) return new JsonResult(ActionStatusMessage.Account.Auth.Permission.SystemAllReadyValid);
			var currentUser = currentUserService.CurrentUser;
			var canAuthRank = _usersService.CheckAuthorizedToUser(currentUser, targetUser);
			var ua = _userActionServices.Log(UserOperation.ModifyUser, model.UserName, $"通过{currentUser?.Id} 授权审核注册", false, ActionRank.Danger);
			if (canAuthRank < 1) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			targetUser.Application.InvitedBy = model.Valid ? currentUser.Id : BLL.Extensions.UserExtensions.InviteByInvalidValue;
			context.AppUserApplicationInfos.Update(targetUser.Application);
			context.SaveChanges();
			_userActionServices.Status(ua, true);
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
		public async Task<IActionResult> RegisterMutil([FromBody] UsersCreateMutilViewModel model)
		{
			model.Verify.Verify(_verifyService);
			if (!model.Auth.Verify(_authService, currentUserService.CurrentUser?.Id)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var authByUser = _usersService.GetById(model.Auth.AuthByUserID);
			foreach (var m in model.Data.List)
			{
				var actionRecord = _userActionServices.Log(UserOperation.Register, m?.Application?.UserName ?? "NOT Specify", $"{authByUser.Id}批量注册", false, ActionRank.Warning);
				await RegisterSingle(m, authByUser);
				_userActionServices.Status(actionRecord, true);
			}
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 修改用户信息，只要当前登录的或者授权登录的为目标用户的上级，则可修改
		/// </summary>
		/// <param name="model">修改实体</param>
		/// <param name="authByUser"></param>
		/// <returns></returns>
		private async Task ModifySingleUser(UserModifyDataModel model, User authByUser)
		{
			if (model.Application?.UserName == null) throw new ActionStatusMessageException(ActionStatusMessage.UserMessage.NoId);
			var localUser = _usersService.GetById(model.Application.UserName);
			// TODO 因lazyload导致无法加载原信息
			if (localUser == null) throw new ActionStatusMessageException(ActionStatusMessage.UserMessage.NotExist);

			// 获取需要修改的目标用户
			if (model.Company == null) throw new ActionStatusMessageException(ActionStatusMessage.CompanyMessage.NotExist);
			var newUser = model.ToModel(authByUser.Id, context.AdminDivisions, context.ThirdpardAccounts);
			var to_modify_NewUser = await _usersService.ModifyAsync(newUser, false);

			// 检查修改后的用户的权限
			var invalidAccount = localUser.Application.InvalidAccount();
			var canAuthRank = _usersService.CheckAuthorizedToUser(authByUser, to_modify_NewUser);
			if (invalidAccount != AccountType.Deny && canAuthRank < (int)invalidAccount) throw new ActionStatusMessageException(new ApiResult(ActionStatusMessage.Account.Auth.Invalid.Default, $"修改后权限不足，仍缺少{(int)invalidAccount - canAuthRank}级权限", true));
			canAuthRank = _usersService.CheckAuthorizedToUser(authByUser, localUser);
			if (invalidAccount != AccountType.Deny && canAuthRank < (int)invalidAccount) throw new ActionStatusMessageException(new ApiResult(ActionStatusMessage.Account.Auth.Invalid.Default, $"修改前权限不足，仍缺少{(int)invalidAccount - canAuthRank}级权限", true));
			to_modify_NewUser.Application.Create = localUser.Application.Create; // 注册日期不允许修改
			CheckCurrentUserData(to_modify_NewUser);
			if (invalidAccount == AccountType.Deny) to_modify_NewUser.Application.InvitedBy = null;//  重新提交
			_logger.LogInformation($"用户信息被修改:{to_modify_NewUser.Id}");
			context.Entry(localUser).State = EntityState.Detached;
			context.AppUsers.Update(to_modify_NewUser);
			await context.SaveChangesAsync();
		}

		private async Task RegisterSingle(UserCreateDataModel model, User authByUser)
		{
			var regUser = _usersService.GetById(model.Application.UserName);
			if (regUser != null) throw new ActionStatusMessageException(ActionStatusMessage.Account.Register.UserExist);
			var username = model.Application.UserName;
			var checkIfCidIsUsed = context.AppUsersDb.Where(u => u.BaseInfo.Cid == model.Base.Cid).FirstOrDefault();
			if (checkIfCidIsUsed != null) throw new ActionStatusMessageException(ActionStatusMessage.Account.Register.CidExist);
			var user = await _usersService.CreateAsync(model.ToModel(authByUser.Id, context.AdminDivisions, context.ThirdpardAccounts), model.Password, CheckCurrentUserData);
			if (!ModelState.IsValid)
				throw new ModelStateException(new ModelStateExceptionViewModel(ModelState));
			if (user == null) throw new ActionStatusMessageException(ActionStatusMessage.Account.Register.Default);
			var toRegisterUser = _usersService.GetById(user.UserName);
			var cmpCode = toRegisterUser?.CompanyInfo?.CompanyCode;
			if (cmpCode == null || cmpCode == "" || cmpCode.ToLower() == "root")
			{
				if (authByUser?.Id != "root") ModelState.AddModelError("用户", ActionStatusMessage.Account.Register.RootCompanyRequireAdminRight.Message);
			}

			// 若当前用户不具有权限，不允许邀请注册时通过
			if (toRegisterUser.Application.InvitedBy != null)
			{
				var authUserPermissionRank = _usersService.CheckAuthorizedToUser(authByUser, toRegisterUser);
				if (authUserPermissionRank < 1)
				{
					toRegisterUser.Application.InvitedBy = null;
					context.AppUserApplicationInfos.Update(toRegisterUser.Application);
					context.SaveChanges();
				}
			}
			var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
			await _emailSender.SendEmailConfirmationAsync(user.Email, callbackUrl);
			_logger.LogInformation($"新的用户创建:{user.UserName}");
		}

		private bool CheckCurrentUserData(User currentUser)
		{
			var reason = string.Empty;
			if (!(currentUser.BaseInfo.Cid?.CheckIDCard(out reason)??false)) ModelState.AddModelError("基本信息", $"身份证号有误:{reason}");
			if (currentUser.BaseInfo.RealName == null || currentUser.BaseInfo.RealName.Length > 8 || currentUser.BaseInfo.RealName.Length <= 1) ModelState.AddModelError("基本信息", $"真实姓名[{currentUser.BaseInfo.RealName}]无效");
			//if (currentUser.BaseInfo.Hometown == null) ModelState.AddModelError("基本信息", "籍贯信息未填写"); // 已在Model中校验过
			//if (currentUser.Application.Email == null || currentUser.Application.Email == "") ModelState.AddModelError("系统信息","认证邮箱为空");
			if (currentUser.BaseInfo.Time_Work.Year < 1950) ModelState.AddModelError("基本信息", "工作时间格式错误");
			if (currentUser.BaseInfo.Time_BirthDay.Year < 1900) ModelState.AddModelError("基本信息", "出生日期格式错误");
			//if (currentUser.BaseInfo.Time_Party.Year < 1950) ModelState.AddModelError("基本信息", "党团时间格式错误");
			if (currentUser.CompanyInfo.Company == null) ModelState.AddModelError("单位信息", "单位不存在");
			if (currentUser.CompanyInfo.Duties == null) ModelState.AddModelError("单位信息", "职务不存在");
			if (currentUser.CompanyInfo.Title == null) ModelState.AddModelError("单位信息", "职务等级不存在");
			var anyCodeInvalid = currentUser.SocialInfo.Settle.AnyCodeInvalid();
			if (currentUser.SocialInfo.Phone == null || currentUser.SocialInfo.Phone.Length < 5) ModelState.AddModelError("家庭情况", "联系方式未正确填写");
			if (anyCodeInvalid != null) ModelState.AddModelError("家庭情况", $"无效的行政区划:{anyCodeInvalid}");
			return ModelState.IsValid;
		}

		#endregion Rest
	}
}