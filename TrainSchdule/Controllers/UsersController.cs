using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BLL.Extensions;
using BLL.Extensions.ApplyExtensions.ApplyAuditStreamExtension;
using BLL.Helpers;
using BLL.Interfaces;
using Castle.Core.Internal;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.Extensions.Users;
using TrainSchdule.ViewModels.User;
using TrainSchdule.ViewModels.Verify;
using static BLL.Services.UsersService;

namespace TrainSchdule.Controllers
{
	/// <summary>
	/// 用户管理
	/// </summary>
	[Authorize]
	[Route("[controller]/[action]")]
	public partial class UsersController : Controller
	{
		#region Fields

		private readonly IUsersService _usersService;
		private readonly ICurrentUserService _currentUserService;
		private readonly ICompaniesService _companiesService;
		private readonly IApplyService _applyService;
		private readonly IGoogleAuthService _authService;
		private readonly ApplicationDbContext _context;
		private readonly IUserActionServices _userActionServices;
		private readonly IHostingEnvironment _hostingEnvironment;

		#endregion Fields

		#region .ctors

		/// <summary>
		/// 用户管理
		/// </summary>
		/// <param name="usersService"></param>
		/// <param name="currentUserService"></param>
		/// <param name="companiesService"></param>
		/// <param name="applyService"></param>
		/// <param name="authService"></param>
		/// <param name="companyManagerServices"></param>
		/// <param name="userActionServices"></param>
		/// <param name="context"></param>
		/// <param name="hostingEnvironment"></param>
		public UsersController(IUsersService usersService, ICurrentUserService currentUserService, ICompaniesService companiesService, IApplyService applyService, IGoogleAuthService authService, ICompanyManagerServices companyManagerServices, IUserActionServices userActionServices, ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
		{
			_usersService = usersService;
			_currentUserService = currentUserService;
			_companiesService = companiesService;
			_applyService = applyService;
			_authService = authService;
			_companyManagerServices = companyManagerServices;
			_userActionServices = userActionServices;
			_context = context;
			_hostingEnvironment = hostingEnvironment;
		}

		#endregion .ctors

		#region Logic

		private User GetCurrentQueryUser(string id, out JsonResult result)
		{
			id = id.IsNullOrEmpty() ? _currentUserService.CurrentUser?.Id : id;
			if (id == null)
			{
				result = new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
				return null;
			}
			var targetUser = _usersService.Get(id);
			if (targetUser == null)
			{
				result = new JsonResult(ActionStatusMessage.User.NotExist);
				return null;
			}
			result = null;
			return targetUser;
		}

		/// <summary>
		/// 系统信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(UserApplicationInfoViewModel), 0)]
		public IActionResult Application(string id)
		{
			var targetUser = GetCurrentQueryUser(id, out var result);
			if (targetUser == null) return result;
			return new JsonResult(new UserApplicationInfoViewModel()
			{
				Data = targetUser.Application.ToModel(targetUser)
			});
		}

		/// <summary>
		/// 用户自定义信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet]
		[ProducesResponseType(typeof(UserDiyInfoViewModel), 0)]
		public IActionResult DiyInfo(string id)
		{
			var targetUser = GetCurrentQueryUser(id, out var result);
			if (targetUser == null) return result;
			return new JsonResult(new UserDiyInfoViewModel()
			{
				Data = targetUser.DiyInfo?.ToViewModel(targetUser, _hostingEnvironment)
			});
		}

		/// <summary>
		/// 用户自定义信息
		/// </summary>
		/// <param name="id"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost]
		[ProducesResponseType(typeof(UserDiyInfoViewModel), 0)]
		public IActionResult DiyInfo(string id, [FromBody] UserDiyInfoModefyModel model)
		{
			var targetUser = GetCurrentQueryUser(id, out var result);
			if (targetUser == null) return result;
			if (!model.Auth.Verify(_authService, _currentUserService.CurrentUser?.Id)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var authByUser = _usersService.Get(model.Auth.AuthByUserID);
			if (id != targetUser.Id && !_userActionServices.Permission(authByUser.Application.Permission, DictionaryAllPermission.User.Application, Operation.Update, authByUser.Id, targetUser.CompanyInfo.Company.Code)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			targetUser.DiyInfo = model.Data.ToModel();
			_usersService.Edit(targetUser);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 社会信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet]
		[ProducesResponseType(typeof(UserSocialViewModel), 0)]
		public IActionResult Social(string id)
		{
			var targetUser = GetCurrentQueryUser(id, out var result);
			if (targetUser == null) return result;
			return new JsonResult(new UserSocialViewModel()
			{
				Data = targetUser.SocialInfo.ToDataModel()
			});
		}

		/// <summary>
		/// 职务信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet]
		[ProducesResponseType(typeof(UserDutiesViewModel), 0)]
		public IActionResult Duties(string id)
		{
			var targetUser = GetCurrentQueryUser(id, out var result);
			if (targetUser == null) return result;
			return new JsonResult(new UserDutiesViewModel()
			{
				Data = targetUser.CompanyInfo.ToDutiesModel()
			});
		}

		/// <summary>
		/// 单位信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet]
		[ProducesResponseType(typeof(UserCompanyInfoViewModel), 0)]
		public IActionResult Company(string id)
		{
			var targetUser = GetCurrentQueryUser(id, out var result);
			if (targetUser == null) return result;
			return new JsonResult(new UserCompanyInfoViewModel()
			{
				Data = targetUser.CompanyInfo.ToCompanyModel(_companiesService)
			});
		}

		/// <summary>
		/// 获取用户简要信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet]
		[ProducesResponseType(typeof(UserSummaryViewModel), 0)]
		public IActionResult Summary(string id)
		{
			var targetUser = GetCurrentQueryUser(id, out var result);
			if (result != null) return result;
			var data = targetUser.ToSummaryDto();
			data.LastLogin = _context.UserActions.Where(u => u.UserName == id).Where(u => u.Operation == UserOperation.Login).Where(u => u.Success == true).OrderByDescending(u => u.Date).FirstOrDefault();
			return new JsonResult(new UserSummaryViewModel()
			{
				Data = data
			});
		}

		/// <summary>
		/// 基础信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet]
		[ProducesResponseType(typeof(UserBaseInfoViewModel), 0)]
		public IActionResult Base(string id)
		{
			var targetUser = GetCurrentQueryUser(id, out var result);
			if (targetUser == null) return result;
			//if (id != null && id != _currentUserService.CurrentUser?.Id) targetUser.BaseInfo.Cid = "***";
			return new JsonResult(new UserBaseInfoWithIdViewModel()
			{
				Data = new UserBaseInfoWithIdDataModel()
				{
					Base = targetUser.BaseInfo,
					Id = targetUser.Id
				}
			});
		}

		/// <summary>
		/// 此用户提交申请后，将生成的审批流
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet]
		[ProducesResponseType(typeof(UserAuditStreamDataModel), 0)]
		public IActionResult AuditStream(string id)
		{
			var targetUser = GetCurrentQueryUser(id, out var result);
			if (targetUser == null) return result;
			var a = new DAL.Entities.ApplyInfo.Apply()
			{
				BaseInfo = new DAL.Entities.ApplyInfo.ApplyBaseInfo()
				{
					From = targetUser
				}
			};
			try
			{
				_applyService.InitAuditStream(a);
			}
			catch (Exception ex)
			{
				return new JsonResult(new ApiResult(-1404, ex.Message));
			}
			return new JsonResult(new UserAuditStreamViewModel()
			{
				Data = new UserAuditStreamDataModel()
				{
					Steps = a.ApplyAllAuditStep.Select(s => s.ToDtoModel()),
					SolutionName = a.ApplyAuditStreamSolutionRule.Solution.Name
				}
			});
		}

		/// <summary>
		/// 获取用户休假情况
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[ProducesResponseType(typeof(UserVocationInfoViewModel), 0)]
		[AllowAnonymous]
		[HttpGet]
		public IActionResult Vocation(string id)
		{
			var targetUser = GetCurrentQueryUser(id, out var result);
			if (targetUser == null) return result;
			var vocationInfo = _usersService.VocationInfo(targetUser);
			return new JsonResult(new UserVocationInfoViewModel()
			{
				Data = vocationInfo
			});
		}

		/// <summary>
		/// 修改头像
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[ProducesResponseType(typeof(ApiResult), 0)]
		[HttpPost]
		public async Task<IActionResult> Avatar([FromBody]ResponseImgDataModel model)
		{
			var targetUser = GetCurrentQueryUser(null, out var result);
			if (result != null) return result;
			try
			{
				await _usersService.UpdateAvatar(targetUser, model.Url).ConfigureAwait(true);
			}
			catch (FileTooLargeException ex)
			{
				return new JsonResult(new ApiResult(ActionStatusMessage.Static.FileSizeInvalid, ex.Message, true));
			}
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 获取头像
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="avatarId">如果传入了此字段则直接读取头像</param>
		/// <returns></returns>
		[AllowAnonymous]
		[ProducesResponseType(typeof(AvatarViewModel), 0)]
		[HttpGet]
		public async Task<IActionResult> Avatar(string userId, string avatarId)
		{
			Avatar avatar = null;
			var targetUser = GetCurrentQueryUser(userId, out var result);
			if (avatarId == null)
			{
				if (result != null) return result;
				avatar = targetUser?.DiyInfo?.Avatar;
				if (avatar == null)
				{
					avatar = targetUser.BaseInfo.RealName.CreateTempAvatar(targetUser.BaseInfo.Gender, _hostingEnvironment.WebRootPath);
					await _usersService.UpdateAvatar(targetUser, avatar?.Img?.ToBase64()).ConfigureAwait(true);
				}
			}
			else
			{
				avatar = _context.AppUserDiyAvatars.Where(a => a.Id.ToString() == avatarId).FirstOrDefault();
			}
			return new JsonResult(new AvatarViewModel()
			{
				Data = avatar.ToModel()
			});
		}

		/// <summary>
		/// 获取用户历史头像
		/// </summary>
		/// <param name="userid"></param>
		/// <param name="start"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet]
		public IActionResult Avatars(string userid, DateTime start)
		{
			var list = _usersService.QueryAvatar(userid, start);
			return new JsonResult(new AvatarListViewModel()
			{
				Data = new AvatarListDataModel()
				{
					List = list.Select(a => a.ToModel())
				}
			});
		}

		#endregion Logic
	}
}