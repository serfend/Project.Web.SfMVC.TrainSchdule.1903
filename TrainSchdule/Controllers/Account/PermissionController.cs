using BLL.Helpers;
using BLL.Interfaces.Permission;
using DAL.Entities;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Account;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers
{
	public partial class AccountController
	{
		private readonly IPermissionServices permissionServices;

		/// <summary>
		/// 系统所有权限列表
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult PermissionDictionary()
		{
			var t = permissionServices.AllPermission();
			return new JsonResult(new EntityViewModel<List<Tuple<string, PermissionDescription>>>(t));
		}

		/// <summary>
		/// 获取当前用户的权限
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(IDictionary<string, PermissionRegion>), 0)]
		public IActionResult Permission(string id)
		{
			var currentId = id ?? currentUserService.CurrentUser.Id;
			if (currentId == null) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			var targetUser = _usersService.GetById(currentId);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
			var permission = targetUser.Application.Permission;
			return new JsonResult(new QueryPermissionsOutViewModel() { Data = permission.GetRegionList() });
		}

		/// <summary>
		/// 修改权限情况
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(typeof(ApiResult), 0)]
		public IActionResult Permission([FromBody] ModifyPermissionsViewModel model)
		{
			if (!model.Auth.Verify(_authService, currentUserService.CurrentUser?.Id)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var targetUser = _usersService.GetById(model.Id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
			var authUser = _usersService.GetById(model.Auth.AuthByUserID);
			if (authUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
			var ua = _userActionServices.Log(UserOperation.Permission, targetUser.Id, $"通过{authUser.Id}");
			// TODO 单位管理或主管可直接编辑本单位权限
			if (!targetUser.Application.Permission.Update(model.NewPermission, authUser.Application.Permission)) return new JsonResult(_userActionServices.LogNewActionInfo(ua, ActionStatusMessage.Account.Auth.Invalid.Default));
			_usersService.Edit(targetUser);
			return new JsonResult(ActionStatusMessage.Success);
		}
	}
}