using System;
using System.Linq;
using BLL.Helpers;
using DAL.Entities.ApplyInfo;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.Apply;

namespace TrainSchdule.Controllers.Apply
{
	public partial class ApplyController
	{
		[HttpPut]
		public IActionResult Save(string id)
		{
			var modelCheck = CheckApplyModelAndDoTask(id, (x) => _applyService.ModifyAuditStatus(x, AuditStatus.NotPublish));
			if (modelCheck.status == ActionStatusMessage.Fail.status) return new JsonResult(ActionStatusMessage.Apply.Operation.Save.AllReadySave);
			return new JsonResult(modelCheck);
		}
		[HttpPut]
		public IActionResult Publish(string id)
		{
			var modelCheck = CheckApplyModelAndDoTask(id, (x) => _applyService.ModifyAuditStatus(x, AuditStatus.Auditing));
			if (modelCheck.status == ActionStatusMessage.Fail.status) return new JsonResult(ActionStatusMessage.Apply.Operation.Publish.AllReadyPublish);
			return new JsonResult(modelCheck);
		}

		[HttpPut]
		public IActionResult Withdrew(string id)
		{
			var modelCheck = CheckApplyModelAndDoTask(id, (x) => _applyService.ModifyAuditStatus(x, AuditStatus.Withdrew));
			if(modelCheck.status==ActionStatusMessage.Fail.status)return new JsonResult(ActionStatusMessage.Apply.Operation.Withdrew.AllReadyWithdrew);
			return new JsonResult(modelCheck);
		}
		private Status CheckApplyModelAndDoTask(string id,Func<DAL.Entities.ApplyInfo.Apply,bool>callBack)
		{
			var apply = _applyService.Get(Guid.Parse(id));
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


		[HttpPost]
		public IActionResult Audit([FromBody]AuditApplyViewModel model)
		{
			if(model.Auth==null||!_authService.Verify(model.Auth.Code,model.Auth.AuthByUserID))return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			try
			{
				var unused = _applyService.Audit(model.ToAuditVDTO(_usersService, _applyService));
			}
			catch (ActionStatusMessageException e)
			{
				return new JsonResult(e.Status);
			}
			return new JsonResult(ActionStatusMessage.Success);
		}

	}
}
