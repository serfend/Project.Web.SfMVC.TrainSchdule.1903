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
        public IActionResult Info(UserProfileViewModel model,string username = null)
        {
	        if (ModelState.IsValid)
	        {
		        if (User.Identity.IsAuthenticated)
		        {
					username = username.IsNullOrEmpty() ? _currentUserService.CurrentUserDTO.UserName : username;
					var item = _usersService.Get(username);
					if (item.Privilege > _currentUserService.CurrentUser.Privilege&&item.UserName!=User.Identity.Name) return new JsonResult(ActionStatusMessage.AccountAuth_Forbidden);

					var rst = new StringBuilder();
					_usersService.Edit(username,async (u) =>
					{
						if(model.Address!=null)u.Address=model.Address;
						if (model.Company != null)
						{
							var cmp =_companiesService.GetCompanyByPath(model.Company);
							u.Company = cmp;
						}

						if (model.Duties != null)
						{
							var duties = _unitOfWork.Duties.Find(x => x.Name == model.Duties)?.FirstOrDefault();
							u.Duties = duties;
							if (duties == null)
							{
								rst.AppendLine($"当前不存在对应职务:{model.Duties}，同时已创建此职务并交管理员审核");
								duties = new Duties() { Name = model.Duties };
								await _unitOfWork.Duties.CreateAsync(duties);
							}
						}
						
						if(model.Phone!=null)u.Phone=model.Phone;
						if(model.RealName!=null)u.RealName=model.RealName;
						if (u.Company == null) rst.AppendLine($"当前不存在对应单位:{model.Company}");
					});
					return rst.Length==0 ? new JsonResult(ActionStatusMessage.Success) : 
						new JsonResult(new Status(ActionStatusMessage.AccountAuth_InvalidByMutilError.status, rst.ToString()));
		        }
				else return new JsonResult(ActionStatusMessage.AccountLogin_InvalidAuth);
			}
	        else
	        {
				return new JsonResult(new Status(ActionStatusMessage.AccountLogin_InvalidByUnknown.status, JsonConvert.SerializeObject(ModelState.AllModelStateErrors())));
			}
        }

        [HttpGet]
        public IActionResult Info(string username=null)
        {
	        if(!User.Identity.IsAuthenticated)return new JsonResult(ActionStatusMessage.AccountAuth_Invalid);
			username =username.IsNullOrEmpty() ? _currentUserService.CurrentUserDTO.UserName:username;
			var item=_usersService.Get(username);
			if (item.Privilege>_currentUserService.CurrentUser.Privilege && item.UserName != User.Identity.Name) return new JsonResult(ActionStatusMessage.AccountAuth_Forbidden);
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