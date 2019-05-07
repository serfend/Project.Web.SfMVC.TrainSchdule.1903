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
	[Authorize]
	[Route("[controller]/[action]")]
    public class UsersController : Controller
    {
        #region Fields

        private readonly IUsersService _usersService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICompaniesService _companiesService;
        private readonly IApplyService _applyService;

		private bool _isDisposed;

        #endregion

        #region .ctors

        public UsersController(IUsersService usersService, ICurrentUserService currentUserService, ICompaniesService companiesService, IApplyService applyService)
        {
            _usersService = usersService;
            _currentUserService = currentUserService;
            _companiesService = companiesService;
            _applyService = applyService;
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
		/// <summary>
		/// 此用户所管辖的单位
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		public IActionResult OnMyManage(string id)
		{
			id = id.IsNullOrEmpty() ? _currentUserService.CurrentUser?.Id : id;
			var targetUser = _usersService.Get(id);
			if(targetUser==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			return new JsonResult(new UserManageRangeViewModel()
			{
				Data = new UserManageRangeDataModel()
				{
					List = _usersService.InMyManage(id).Select(c=>c.ToDTO(_companiesService))
				}
			});
		}
		#endregion

		#region Disposing

		protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _usersService.Dispose();
                    _currentUserService.Dispose();
                }

                _isDisposed = true;

                base.Dispose(disposing);
            }
        }

        #endregion
    }
}