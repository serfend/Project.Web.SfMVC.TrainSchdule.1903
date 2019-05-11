using System;
using System.Linq;
using BLL.Helpers;
using DAL.Entities.ApplyInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.Apply;

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
			var modelCheck = CheckApplyModelAndDoTask(id, (x) => _applyService.ModifyAuditStatus(x, AuditStatus.NotPublish));
			if (modelCheck.status == ActionStatusMessage.Fail.status) return new JsonResult(ActionStatusMessage.Apply.Operation.Save.AllReadySave);
			return new JsonResult(modelCheck);
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
			var modelCheck = CheckApplyModelAndDoTask(id, (x) => _applyService.ModifyAuditStatus(x, AuditStatus.Auditing));
			if (modelCheck.status == ActionStatusMessage.Fail.status) return new JsonResult(ActionStatusMessage.Apply.Operation.Publish.AllReadyPublish);
			return new JsonResult(modelCheck);
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
			var modelCheck = CheckApplyModelAndDoTask(id, (x) => _applyService.ModifyAuditStatus(x, AuditStatus.Withdrew));
			if(modelCheck.status==ActionStatusMessage.Fail.status)return new JsonResult(ActionStatusMessage.Apply.Operation.Withdrew.AllReadyWithdrew);
			return new JsonResult(modelCheck);
		}
		
		private Status CheckApplyModelAndDoTask(string id,Func<DAL.Entities.ApplyInfo.Apply,bool>callBack)
		{
			Guid.TryParse(id, out var gid);
			var apply = _applyService.Get(gid);
			if (apply == null) return ActionStatusMessage.Apply.NotExist;
			var userid = _currentUserService.CurrentUser?.Id;
			if (userid == null) return ActionStatusMessage.Account.Auth.Invalid.NotLogin;
			if (apply.BaseInfo.From.Id != userid)
			{
				if (apply.Response.All(r => !_companiesService.CheckManagers(r.Company.Code, userid))) return ActionStatusMessage.Account.Auth.Invalid.Default;
			}

			if (callBack.Invoke(apply))return ActionStatusMessage.Success;
			return ActionStatusMessage.Fail;
		}

		/// <summary>
		/// 审核申请
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(Status), 0)]

		public IActionResult Audit([FromBody]AuditApplyViewModel model)
		{
			if(model.Auth==null||!_authService.Verify(model.Auth.Code,model.Auth.AuthByUserID))return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
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
