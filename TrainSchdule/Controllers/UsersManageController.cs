using System.Linq;
using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.DTO.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.ViewModels.User;

namespace TrainSchdule.Controllers
{
	public partial class UsersController
	{
		private readonly ICompanyManagerServices _companyManagerServices;
		/// <summary>
		/// 此用户所管辖的单位
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(UserManageRangeDataModel), 0)]

		public IActionResult OnMyManage(string id)
		{
			id=id ?? _currentUserService.CurrentUser?.Id;
			var targetUser = _usersService.Get(id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			return new JsonResult(new UserManageRangeViewModel()
			{
				Data = new UserManageRangeDataModel()
				{
					List = _usersService.InMyManage(id).Select(c => c.ToDTO(_companiesService))
				}
			});
		}
		/// <summary>
		/// 移除管辖单位
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpDelete]
		[AllowAnonymous]
		[ProducesResponseType(typeof(Status), 0)]

		public IActionResult OnMyManage([FromBody] UserManageRangeModifyViewModel model)
		{
			if(model.Auth==null||!_authService.Verify(model.Auth.Code,model.Auth.AuthByUserID))return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var id = model.Id ?? _currentUserService.CurrentUser?.Id;
			var targetUser = _usersService.Get(id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var manage = _companyManagerServices.GetManagerByUC(model.Id, model.Code);
			if(manage==null)return new JsonResult(ActionStatusMessage.Company.Manager.NotExist);
			var unused=_companyManagerServices.Delete(manage);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 新增管辖单位
		/// </summary>
		/// <param name="model"></param>
		/// <param name="mdzz">填充参数，无需填写</param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(Status), 0)]

		public IActionResult OnMyManage([FromBody] UserManageRangeModifyViewModel model,string mdzz)
		{
			if (!_authService.Verify(model.Auth.Code, model.Auth.AuthByUserID)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var id = model.Id ?? _currentUserService.CurrentUser?.Id;
			var targetUser = _usersService.Get(id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var manage = _companyManagerServices.GetManagerByUC(model.Id, model.Code);
			if (manage != null) return new JsonResult(ActionStatusMessage.Company.Manager.Existed);
			var dto = new CompanyManagerVdto()
			{
				AuditById = model.Auth.AuthByUserID,
				CompanyCode = model.Code,
				UserId = model.Id
			};
			manage = _companyManagerServices.CreateManagers(dto);
			if (manage == null) return new JsonResult(ActionStatusMessage.Company.Manager.Default);
			return new JsonResult(ActionStatusMessage.Success);
		}
	}
}
