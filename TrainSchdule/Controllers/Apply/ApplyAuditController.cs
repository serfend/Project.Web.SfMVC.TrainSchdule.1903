using System;
using System.Linq;
using BLL.Helpers;
using DAL.Entities.ApplyInfo;
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
		[ProducesResponseType(typeof(Status),0)]

		public IActionResult Save(string id)
		{
			try
			{
				CheckApplyModelAndDoTask(id, (x) => _applyService.ModifyAuditStatus(x, AuditStatus.NotPublish));
			}
			catch (ActionStatusMessageException e)
			{
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

		[ProducesResponseType(typeof(Status), 0)]

		public IActionResult Publish(string id)
		{
			try
			{
				CheckApplyModelAndDoTask(id, (x) => _applyService.ModifyAuditStatus(x, AuditStatus.Auditing));
			}
			catch (ActionStatusMessageException e)
			{
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

		[ProducesResponseType(typeof(Status), 0)]

		public IActionResult Withdrew(string id)
		{
			try
			{
				CheckApplyModelAndDoTask(id, (x) => _applyService.ModifyAuditStatus(x, AuditStatus.Withdrew));
			}
			catch (ActionStatusMessageException e)
			{
				return new JsonResult(e.Status);
			}
			return new JsonResult(ActionStatusMessage.Success);
		}
		
		private void CheckApplyModelAndDoTask(string id,Action<DAL.Entities.ApplyInfo.Apply>callBack)
		{
			Guid.TryParse(id, out var gid);
			var apply = _applyService.Get(gid);
			if (apply == null) throw new ActionStatusMessageException(ActionStatusMessage.Apply.NotExist);
			var userid = _currentUserService.CurrentUser?.Id;
			if (userid == null) throw new ActionStatusMessageException(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			if (apply.BaseInfo.From.Id != userid)
			{
				if (apply.Response.All(r => !_companiesService.CheckManagers(r.Company.Code, userid))) throw new ActionStatusMessageException(ActionStatusMessage.Account.Auth.Invalid.Default);
			}
			callBack.Invoke(apply);
		}

		/// <summary>
		/// 审核申请(可使用登录状态直接授权，也可使用授权人）
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(Status), 0)]

		public IActionResult Audit([FromBody]AuditApplyViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			var currentUserName = _currentUserService.HttpContextAccessor.HttpContext.User.Identity.Name;
			if( !model.Auth.Verify(_authService))currentUserName=model.Auth.AuthByUserID;
			if (currentUserName == null) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			model.Auth.AuthByUserID = currentUserName;
			try
			{
				model.Data.List = model.Data.List.Distinct(new CompareAudit());
				var results = _applyService.Audit(model.ToAuditVDTO(_usersService, _applyService));
				int count = 0;
				return new JsonResult(new ApplyAuditResponseStatusViewModel()
				{
					Data = results.Select(r=>new ApplyAuditResponseStatusDataModel(model.Data.List.ElementAt(count++).Id,r))
				});
			}
			catch (ActionStatusMessageException e)
			{
				return new JsonResult(e.Status);
			}
		}

	}
}
