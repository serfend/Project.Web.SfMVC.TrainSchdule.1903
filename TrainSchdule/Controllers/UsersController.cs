using System.Linq;
using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using Castle.Core.Internal;
using DAL.Entities;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.ViewModels.User;
using TrainSchdule.ViewModels.Verify;

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

		#endregion

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
		public UsersController(IUsersService usersService, ICurrentUserService currentUserService, ICompaniesService companiesService, IApplyService applyService, IGoogleAuthService authService, ICompanyManagerServices companyManagerServices)
		{
			_usersService = usersService;
			_currentUserService = currentUserService;
			_companiesService = companiesService;
			_applyService = applyService;
			_authService = authService;
			_companyManagerServices = companyManagerServices;
		}

		#endregion

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
				result= new JsonResult(ActionStatusMessage.User.NotExist);
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
		public IActionResult DiyInfo(string id,[FromBody] UserDiyInfoModefyModel model)
		{
			var targetUser = GetCurrentQueryUser(id, out var result);
			if (targetUser == null) return result;
			if (!model.Verify(_authService)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var authByUser = _usersService.Get(model.AuthByUserID);
			if (id!=targetUser.Id&&!authByUser.Application.Permission.Check(DictionaryAllPermission.User.Application, Operation.Update, targetUser)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			targetUser.DiyInfo = model.Data.ToModel(targetUser.DiyInfo);
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
			return new JsonResult(new UserBaseInfoViewModel()
			{
				Data = targetUser.BaseInfo
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
			var list = _applyService.GetAuditStream(targetUser.CompanyInfo.Company);
			return new JsonResult(new UserAuditStreamViewModel()
			{
				Data = new UserAuditStreamDataModel()
				{
					List = list.Select(c => c.Company.ToDto(_companiesService))
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
		#endregion

	}
}