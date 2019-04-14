using System.Linq;
using System.Text;
using Castle.Core.Internal;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.BLL.DTO;
using TrainSchdule.BLL.Helpers;
using TrainSchdule.BLL.Interfaces;
using TrainSchdule.DAL.Interfaces;
using TrainSchdule.ViewModels.User;
using TrainSchdule.WEB.Extensions;

namespace TrainSchdule.WEB.Controllers
{
	[Route("[controller]/[action]")]
    public class UsersController : Controller
    {
        #region Fields

        private readonly IUsersService _usersService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICompanieservice _companiesService;
        private readonly IUnitOfWork _unitOfWork;

        private bool _isDisposed;

        #endregion

        #region .ctors

        public UsersController(IUsersService usersService, ICurrentUserService currentUserService, ICompanieservice companiesService, IUnitOfWork unitOfWork)
        {
            _usersService = usersService;
            _currentUserService = currentUserService;
            _companiesService = companiesService;
            _unitOfWork = unitOfWork;
        }

		#endregion

		#region Logic
		[HttpGet,Route("{username}")]
        public ActionResult Details(string username)
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
						u.Address=model.Address;
						var cmp=_companiesService.GetCompanyByPath(model.Company);
						u.Company = cmp;
						var duties = _unitOfWork.Duties.Find(x => x.Name == model.Duties)?.FirstOrDefault();
						u.Duties=duties;
						if (duties == null)
						{
							rst.AppendLine($"当前不存在对应职务:{model.Duties}，同时已创建此职务并交管理员审核");
							duties = new Duties() { Name = model.Duties };
							await _unitOfWork.Duties.CreateAsync(duties);
						}
						u.Phone=model.Phone;
						u.RealName=model.RealName;
						if (u.Company == null) rst.AppendLine($"当前不存在对应单位:{model.Company}");
					});
					return rst.Length==0 ? new JsonResult(ActionStatusMessage.Success) : 
						new JsonResult(new Status(ActionStatusMessage.AccountAuth_InvalidByMutilError.Code, rst.ToString()));
		        }
				else return new JsonResult(ActionStatusMessage.AccountLogin_InvalidAuth);
			}
	        else
	        {
		        var rst = new StringBuilder();
		        var all = ModelState.Root.Children.All(child=>child.Errors.All(err => { rst.AppendLine(err.ErrorMessage);
			        return true;
		        }));
		        return new JsonResult(new Status(ActionStatusMessage.AccountLogin_InvalidByUnknown.Code, rst.ToString()));
	        }
        }

        [HttpGet]
        public IActionResult Info(string username=null)
        {
	        if(!User.Identity.IsAuthenticated)return new JsonResult(ActionStatusMessage.AccountAuth_Invalid);
			username =username.IsNullOrEmpty() ? _currentUserService.CurrentUserDTO.UserName:username;
			var item=_usersService.Get(username);
			if (item.Privilege>_currentUserService.CurrentUser.Privilege && item.UserName != User.Identity.Name) return new JsonResult(ActionStatusMessage.AccountAuth_Forbidden);
			return new JsonResult(item);
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