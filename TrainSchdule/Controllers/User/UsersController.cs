using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BLL.Extensions;
using BLL.Extensions.ApplyExtensions.ApplyAuditStreamExtension;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.ApplyInfo;
using Castle.Core.Internal;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.Permisstions;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.Extensions.Users;
using TrainSchdule.Extensions.Users.Social;
using TrainSchdule.ViewModels.Account;
using TrainSchdule.ViewModels.User;
using TrainSchdule.ViewModels.Verify;
using static BLL.Services.UsersService;

namespace TrainSchdule.Controllers
{
	/// <summary>
	/// 用户管理
	/// </summary>
	[Authorize]
	[Route("[controller]")]
	public partial class UsersController : Controller
	{
		private readonly IWebHostEnvironment env;

		#region Fields

		private readonly IUsersService usersService;
		private readonly ICurrentUserService currentUserService;
		private readonly IUserServiceDetail userServiceDetail;
		private readonly ICompaniesService companiesService;
		private readonly IApplyVacationService applyService;
		private readonly IGoogleAuthService authService;
		private readonly ApplicationDbContext context;
		private readonly IUserActionServices userActionServices;

		#endregion Fields

		#region .ctors

		/// <summary>
		/// 用户管理
		/// </summary>
		/// <param name="env"></param>
		/// <param name="usersService"></param>
		/// <param name="currentUserService"></param>
		/// <param name="userServiceDetail"></param>
		/// <param name="companiesService"></param>
		/// <param name="applyService"></param>
		/// <param name="authService"></param>
		/// <param name="companyManagerServices"></param>
		/// <param name="userActionServices"></param>
		/// <param name="context"></param>
		public UsersController(IWebHostEnvironment env, IUsersService usersService, ICurrentUserService currentUserService, IUserServiceDetail userServiceDetail, ICompaniesService companiesService, IApplyVacationService applyService, IGoogleAuthService authService, ICompanyManagerServices companyManagerServices, IUserActionServices userActionServices, ApplicationDbContext context)
		{
			this.env = env;
			this.usersService = usersService;
			this.currentUserService = currentUserService;
			this.userServiceDetail = userServiceDetail;
			this.companiesService = companiesService;
			this.applyService = applyService;
			this.authService = authService;
			this.companyManagerServices = companyManagerServices;
			this.userActionServices = userActionServices;
			this.context = context;
		}

		#endregion .ctors

		#region Logic


		/// <summary>
		/// 系统信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(UserApplicationInfoViewModel), 0)]
		[Route("[action]")]
		public IActionResult Application(string id)
		{
			var targetUser = usersService.CurrentQueryUser(id);
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
		[Route("[action]")]
		public IActionResult DiyInfo(string id)
		{
			var targetUser = usersService.CurrentQueryUser(id);
			return new JsonResult(new UserDiyInfoViewModel()
			{
				Data = targetUser.DiyInfo?.ToViewModel(targetUser)
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
		[Route("[action]")]
		public IActionResult DiyInfo(string id, [FromBody] UserDiyInfoModifyModel model)
		{
			var targetUser = usersService.CurrentQueryUser(id);
			if (!model.Auth.Verify(authService, currentUserService.CurrentUser?.Id)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var authByUser = usersService.GetById(model.Auth.AuthByUserID);
			if (id != targetUser.Id && !userActionServices.Permission(authByUser, ApplicationPermissions.User.CustomeInfo.Item, PermissionType.Write,  targetUser.CompanyInfo.CompanyCode,"修改用户自定义信息")) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			targetUser.DiyInfo = model.Data.ToModel(context.ThirdpardAccounts);
			usersService.Edit(targetUser);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 职务信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet]
		[ProducesResponseType(typeof(UserDutiesViewModel), 0)]
		[Route("[action]")]
		public IActionResult Duties(string id)
		{
			var targetUser = usersService.CurrentQueryUser(id);
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
		[Route("[action]")]
		public IActionResult Company(string id)
		{
			var targetUser = usersService.CurrentQueryUser(id);
			return new JsonResult(new UserCompanyInfoViewModel()
			{
				Data = targetUser.CompanyInfo.ToCompanyModel(companiesService)
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
		[Route("[action]")]
		public IActionResult Summary(string id)
		{
			var targetUser = usersService.CurrentQueryUser(id);
			var data = targetUser.ToSummaryDto();
			return new JsonResult(new UserSummaryViewModel()
			{
				Data = data
			});
		}

		/// <summary>
		/// 上次登录
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet]
		[Route("[action]")]
		public IActionResult LastLogin(string id)
		{
			var targetUser = usersService.CurrentQueryUser(id);
			var data = context.UserActionsDb.Where(u => u.UserName == id).Where(u => u.Operation == UserOperation.Login).Where(u => u.Success == true).OrderByDescending(u => u.Date).ToList();
			return new JsonResult(new UserActionReportViewModel()
			{
				Data = new UserActionDataModel()
				{
					List = data,
					TotalCount = data.Count
				}
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
		[Route("[action]")]
		public IActionResult Base(string id)
		{
			var targetUser = usersService.CurrentQueryUser(id);
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
        /// 获取用户休假情况
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="vacationYear">休假年份</param>
        /// <param name="isPlan">是否查看计划休假的</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(UserVacationInfoViewModel), 0)]
		[AllowAnonymous]
		[HttpGet]
		[Route("[action]")]
		public IActionResult Vacation(string id,int vacationYear,bool isPlan)
		{
			if (vacationYear == 0) vacationYear = DateTime.Now.XjxtNow().Year;
			var targetUser = usersService.CurrentQueryUser(id);
			var vacationInfo = usersService.VacationInfo(targetUser, vacationYear,isPlan?MainStatus.IsPlan:MainStatus.Normal);
			return new JsonResult(new UserVacationInfoViewModel()
			{
				Data = vacationInfo
			});
		}

		/// <summary>
		/// 修改头像
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[ProducesResponseType(typeof(ApiResult), 0)]
		[HttpPost]
		[Route("[action]")]
		public async Task<IActionResult> Avatar([FromBody] ResponseImgDataModel model)
		{
			var targetUser =currentUserService.CurrentUser;
			await usersService.UpdateAvatar(targetUser, model.Url).ConfigureAwait(true);
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
		[Route("[action]")]
		public async Task<IActionResult> Avatar(string userId, string avatarId)
		{
			Avatar avatar = null;
			var targetUser = usersService.CurrentQueryUser(userId);
			if (avatarId == null)
			{
				avatar = targetUser?.DiyInfo?.Avatar;
				if (avatar == null)
				{
					avatar = targetUser.BaseInfo.RealName.CreateTempAvatar(targetUser.BaseInfo.Gender, env.WebRootPath);
					await usersService.UpdateAvatar(targetUser, avatar?.Img?.ToBase64()).ConfigureAwait(true);
				}
			}
			else
			{
				avatar = context.AppUserDiyAvatars.Where(a => a.Id.ToString() == avatarId).FirstOrDefault();
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
		[Route("[action]")]
		public IActionResult Avatars(string userid, DateTime start)
		{
			var list = usersService.QueryAvatar(userid, start);
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