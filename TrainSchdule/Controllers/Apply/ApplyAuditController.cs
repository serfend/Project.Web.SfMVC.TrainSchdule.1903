using System;
using System.Linq;
using BLL.Helpers;
using DAL.Entities.ApplyInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.Controllers.Apply
{
	public partial class ApplyController
	{
		[HttpPut]
		public IActionResult Save(string id)
		{
			var modelCheck = CheckApplyModelAndDoTask(id, (x) => _applyService.ModifyAuditStatus(x, AuditStatus.NotPublish).ToString());
			return modelCheck;
		}
		[HttpPut]
		public IActionResult Publish(string id)
		{
			var modelCheck = CheckApplyModelAndDoTask(id, (x) => _applyService.ModifyAuditStatus(x, AuditStatus.Auditing).ToString());
			return modelCheck;
		}

		[HttpPut]
		public IActionResult Withdrew(string id)
		{
			var modelCheck = CheckApplyModelAndDoTask(id, (x) => _applyService.ModifyAuditStatus(x, AuditStatus.Withdrew).ToString());
			return modelCheck;
		}
		private IActionResult CheckApplyModelAndDoTask(string id,Func<DAL.Entities.ApplyInfo.Apply,string>callBack)
		{
			var apply = _applyService.Get(Guid.Parse(id));
			if (apply == null) return new JsonResult(ActionStatusMessage.Apply.NotExist);
			var userid = _currentUserService.CurrentUser?.Id;
			if (userid == null) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			if (apply.BaseInfo.From.Id != userid)
			{
				if (apply.Response.All(r => !_companiesService.CheckManagers(r.Company.Code, userid))) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			}

			var result = callBack.Invoke(apply);
			return new JsonResult(new APIResponseResultViewModel(result, ActionStatusMessage.Success));
		}
	}
}
