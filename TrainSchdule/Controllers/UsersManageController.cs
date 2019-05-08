using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.DTO.Company;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.ViewModels;
using TrainSchdule.ViewModels.System;
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

		[HttpDelete]
		[AllowAnonymous]
		public IActionResult OnMyManage([FromBody] UserManageRangeModifyViewModel model)
		{
			if(model.Auth==null||!_authService.Verify(model.Auth.Code,model.Auth.AuthByUserID))return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var id = model.Id ?? _currentUserService.CurrentUser?.Id;
			var targetUser = _usersService.Get(id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var manage = _companyManagerServices.GetManagerByUC(model.Id, model.Code);
			if(manage==null)return new JsonResult(ActionStatusMessage.Company.Manager.NotExist);
			int result=_companyManagerServices.Delete(manage);
			return new JsonResult(ActionStatusMessage.Success);
		}

		[HttpPost]
		[AllowAnonymous]
		public IActionResult OnMyManage([FromBody] UserManageRangeModifyViewModel model,string mdzz)
		{
			if (!_authService.Verify(model.Auth.Code, model.Auth.AuthByUserID)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var id = model.Id ?? _currentUserService.CurrentUser?.Id;
			var targetUser = _usersService.Get(id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var manage = _companyManagerServices.GetManagerByUC(model.Id, model.Code);
			if (manage != null) return new JsonResult(ActionStatusMessage.Company.Manager.Existed);
			var dto = new CompanyManagerVDTO()
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
