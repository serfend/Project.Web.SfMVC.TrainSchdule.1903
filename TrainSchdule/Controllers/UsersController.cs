using System.Linq;
using System.Text;
using Castle.Core.Internal;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TrainSchdule.BLL.DTO;
using TrainSchdule.BLL.Helpers;
using TrainSchdule.BLL.Interfaces;
using TrainSchdule.DAL.Interfaces;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.User;
using TrainSchdule.WEB.Extensions;

namespace TrainSchdule.WEB.Controllers
{
	[Authorize]
	[Route("[controller]/[action]")]
    public class UsersController : Controller
    {
        #region Fields

        private readonly IUsersService _usersService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICompaniesService _companiesService;
        private readonly IUnitOfWork _unitOfWork;

        private bool _isDisposed;

        #endregion

        #region .ctors

        public UsersController(IUsersService usersService, ICurrentUserService currentUserService, ICompaniesService companiesService, IUnitOfWork unitOfWork)
        {
            _usersService = usersService;
            _currentUserService = currentUserService;
            _companiesService = companiesService;
            _unitOfWork = unitOfWork;
        }

		#endregion

		#region Logic
		[HttpGet,Route("{username}")]
        public IActionResult Details(string username)
        {
            var item = _usersService.Get(username).ToViewModel();

            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUser = _currentUserService.CurrentUserDTO.ToViewModel();
            }

            return View(item);
        }

        [HttpPost]
        [AllowAnonymous]
		public IActionResult Info(UserProfileViewModel model,string username = null)
        {
	        if (ModelState.IsValid)
	        {
		        if (!User.Identity.IsAuthenticated)return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
					username = username.IsNullOrEmpty() ? _currentUserService.CurrentUserDTO.UserName : username;
					var item = _usersService.Get(username);
					if (item.Privilege > _currentUserService.CurrentUser.Privilege&&item.UserName!=User.Identity.Name) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);

					var rst = new StringBuilder();
					_usersService.Edit(username,async (u) =>
					{
						if(model.Address!=null)u.Address=model.Address;
						if (model.Company != null)
						{
							if (item.Privilege <= _currentUserService.CurrentUser.Privilege)
								rst.AppendLine($"更改用户的所在单位需要更高级别的权限");
							else
							{
								var cmp = _companiesService.GetCompanyByPath(model.Company);
								u.Company = cmp;
							}
						}

						if (model.Duties != null)
						{
							var duties = _unitOfWork.Duties.Find(x => x.Name == model.Duties)?.FirstOrDefault();
							if (duties == null)
							{
								rst.AppendLine($"当前不存在对应职务:{model.Duties}，同时已创建此职务并交管理员审核");
								duties = new Duties() { Name = model.Duties };
								await _unitOfWork.Duties.CreateAsync(duties);
							}
							u.Duties = duties;
						}
						
						if(model.Phone!=null)u.Phone=model.Phone;
						if(model.RealName!=null)u.RealName=model.RealName;
						if (u.Company == null) rst.AppendLine($"当前不存在对应单位:{model.Company}");
					});
					_unitOfWork.Save();
					return rst.Length == 0 ? new JsonResult(ActionStatusMessage.Success) : new JsonResult(new Status(ActionStatusMessage.Fail.status, rst.ToString()));

			}
	        else
	        {
				return new JsonResult(new Status(ActionStatusMessage.Fail.status, JsonConvert.SerializeObject(ModelState.AllModelStateErrors())));
			}
        }

        [HttpGet]
        [AllowAnonymous]
		public IActionResult Info(string username=null)
        {
	        if(!User.Identity.IsAuthenticated)return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			username =username.IsNullOrEmpty() ? _currentUserService.CurrentUserDTO.UserName:username;
			var item=_usersService.Get(username);
			if (item.Privilege>_currentUserService.CurrentUser.Privilege && item.UserName != User.Identity.Name) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			return new JsonResult(new UserDetailViewModel()
			{
				data=item
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