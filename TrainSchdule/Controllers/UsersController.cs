using System.Linq;
using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.ViewModels.User;

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
			if (!User.Identity.IsAuthenticated) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			id = id.IsNullOrEmpty() ? _currentUserService.CurrentUser?.Id : id;
			var targetUser = _usersService.Get(id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			return new JsonResult(new UserApplicationInfoViewModel()
			{
				Data = targetUser.Application.ToModel()
			});
		}
		/// <summary>
		/// 社会信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(UserSocialViewModel), 0)]

		public IActionResult Social(string id)
		{
			id = id.IsNullOrEmpty() ? _currentUserService.CurrentUser?.Id : id;
			var targetUser = _usersService.Get(id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			return new JsonResult(new UserSocialViewModel()
			{
				Data = targetUser.SocialInfo.ToModel()
			});
		}
		/// <summary>
		/// 职务信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(UserDutiesViewModel), 0)]

		public IActionResult Duties(string id)
		{
			id = id.IsNullOrEmpty() ? _currentUserService.CurrentUser?.Id : id;
			var targetUser = _usersService.Get(id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
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
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(UserCompanyInfoViewModel), 0)]

		public IActionResult Company(string id)
		{
			id = id.IsNullOrEmpty() ? _currentUserService.CurrentUser?.Id : id;
			var targetUser = _usersService.Get(id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
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
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(UserBaseInfoViewModel), 0)]

		public IActionResult Base(string id)
		{
			id = id.IsNullOrEmpty() ? _currentUserService.CurrentUser?.Id : id;
			var targetUser = _usersService.Get(id);
			if(targetUser==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			return  new JsonResult(new UserBaseInfoViewModel()
			{
				Data = targetUser.BaseInfo.ToModel(id)
			});
		}
		/// <summary>
		/// 此用户提交申请的审批流
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(UserAuditStreamDataModel), 0)]

		public IActionResult AuditStream(string id)
		{
			id = id.IsNullOrEmpty() ? _currentUserService.CurrentUser?.Id : id;
			var targetUser = _usersService.Get(id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var list = _applyService.GetAuditStream(targetUser.CompanyInfo.Company);
			return new JsonResult(new UserAuditStreamViewModel()
			{
				Data = new UserAuditStreamDataModel()
				{
					List = list.Select(c=>c.Company.ToDTO(_companiesService))
				}
			});
		}
		
		#endregion

    }
}