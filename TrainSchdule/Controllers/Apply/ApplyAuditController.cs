using System;
using System.Linq;
using System.Text;
using BLL.Helpers;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels;
using TrainSchdule.ViewModels.Apply;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers.Apply
{
	public partial class ApplyController
	{
		/// <summary>
		/// 保存申请
		/// </summary>
		/// <param name="id">申请的id</param>
		/// <returns></returns>
		[HttpPut]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResult), 0)]
		public IActionResult Save(string id)
		{
			var ua = _userActionServices.Log(UserOperation.ModifyApply, id, $"保存", false, ActionRank.Infomation);
			try
			{
				CheckApplyModelAndDoTask(id, (x, u) =>
				{
					_applyService.ModifyAuditStatus(x, AuditStatus.NotPublish, u);
					_userActionServices.Status(ua, true, $"通过{u}");
				});
			}
			catch (ActionStatusMessageException e)
			{
				_userActionServices.Status(ua, false, e.Status.Message);
				return new JsonResult(e.Status);
			}
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 发布申请
		/// </summary>
		/// <param name="id">申请的id</param>
		/// <returns></returns>
		[HttpPut]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResult), 0)]
		public IActionResult Publish(string id)
		{
			var ua = _userActionServices.Log(UserOperation.ModifyApply, id, $"发布", false, ActionRank.Infomation);
			try
			{
				CheckApplyModelAndDoTask(id, (x, u) =>
				{
					_applyService.ModifyAuditStatus(x, AuditStatus.Auditing, u);
					_userActionServices.Status(ua, true, $"通过{u}");
				});
			}
			catch (ActionStatusMessageException e)
			{
				_userActionServices.Status(ua, false, e.Status.Message);
				return new JsonResult(e.Status);
			}
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 撤回申请
		/// </summary>
		/// <param name="id">申请的id</param>
		/// <returns></returns>
		[HttpPut]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResult), 0)]
		public IActionResult Withdrew(string id)
		{
			UserAction ua = _userActionServices.Log(UserOperation.ModifyApply, id, $"撤回", false, ActionRank.Warning);
			try
			{
				CheckApplyModelAndDoTask(id, (x, u) =>
				{
					_applyService.ModifyAuditStatus(x, AuditStatus.Withdrew, u);
					_userActionServices.Status(ua, true, $"通过{u}");
				});
			}
			catch (ActionStatusMessageException e)
			{
				_userActionServices.Status(ua, false, e.Status.Message);
				return new JsonResult(e.Status);
			}
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 作废申请
		/// </summary>
		/// <param name="id">申请的id</param>
		/// <returns></returns>
		[HttpPut]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResult), 0)]
		public IActionResult Cancel(string id)
		{
			UserAction ua = _userActionServices.Log(DAL.Entities.UserInfo.UserOperation.ModifyApply, id, "作废休假", false, ActionRank.Danger);
			try
			{
				CheckApplyModelAndDoTask(id, (x, u) =>
				{
					_applyService.ModifyAuditStatus(x, AuditStatus.Cancel, u);
					_userActionServices.Status(ua, true, $"通过{u}");
				}, false);  // 无需授权，因为ModifyAuditStatus已判断权限问题
			}
			catch (ActionStatusMessageException e)
			{
				_userActionServices.Status(ua, false, e.Status.Message);
				return new JsonResult(e.Status);
			}
			return new JsonResult(ActionStatusMessage.Success);
		}

		private void CheckApplyModelAndDoTask(string id, Action<DAL.Entities.ApplyInfo.Apply, string> callBack, bool needPermission = true)
		{
			Guid.TryParse(id, out var gid);
			var apply = _applyService.GetById(gid);
			if (apply == null) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.NotExist);
			var currentUser = _currentUserService.CurrentUser;
			if (currentUser == null) throw new ActionStatusMessageException(ActionStatusMessage.Account.Auth.Invalid.NotLogin);

			if (apply.BaseInfo.From.Id != currentUser?.Id)
			{
				var permit = _userActionServices.Permission(currentUser.Application.Permission, DictionaryAllPermission.Apply.Default, Operation.Update, currentUser.Id, apply.BaseInfo.From.CompanyInfo.Company.Code, "执行休假申请的操作");
				if (!permit && needPermission) throw new ActionStatusMessageException(ActionStatusMessage.Account.Auth.Invalid.Default);
			}
			callBack.Invoke(apply, currentUser.Id);
		}

		/// <summary>
		/// 审核申请(可使用登录状态直接授权，也可使用授权人）
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResult), 0)]
		public IActionResult Audit([FromBody] AuditApplyViewModel model)
		{
			var auditUser = _currentUserService.CurrentUser;
			if (model.Auth?.AuthByUserID != null && auditUser?.Id != model.Auth?.AuthByUserID)
			{
				if (model.Auth.Verify(_authService, _currentUserService.CurrentUser?.Id))
					auditUser = _usersService.GetById(model.Auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}
			try
			{
				var applyStrList = new StringBuilder();
				foreach (var a in model.Data.List) applyStrList.Append(a.Id).Append(':').Append(a.Action).Append(',');
				var ua = _userActionServices.Log(DAL.Entities.UserInfo.UserOperation.AuditApply, auditUser.Id, $"授权审批申请:{applyStrList}", true, ActionRank.Warning);
				model.Data.List = model.Data.List.Distinct(new CompareAudit());
				var results = _applyService.Audit(model.ToAuditVDTO(auditUser, _applyService));
				int count = 0;
				return new JsonResult(new ApplyAuditResponseStatusViewModel()
				{
					Data = results.Select(r => new ApplyAuditResponseStatusDataModel(model.Data.List.ElementAt(count++).Id, r))
				});
			}
			catch (ActionStatusMessageException e)
			{
				return new JsonResult(e.Status);
			}
		}
	}
}