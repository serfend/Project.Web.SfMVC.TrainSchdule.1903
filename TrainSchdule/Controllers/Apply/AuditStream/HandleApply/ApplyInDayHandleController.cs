using BLL.Extensions.ApplyExtensions.ApplyAuditStreamExtension;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.ApplyInfo;
using BLL.Interfaces.Audit;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.ApplyInfo.DailyApply;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.Apply;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers.Apply.AuditStream.HandleApply
{

	/// <summary>
	/// 操作请假
	/// </summary>
	/// 
	[Authorize]
	[Route("applyAudit/[controller]/[action]")]
	public class ApplyInDayHandleController : Controller
	{
		private readonly IUsersService usersService;
		private readonly ICurrentUserService currentUserService;
		private readonly IUserActionServices userActionServices;
		private readonly IAuditStreamServices auditStreamServices;
		private readonly IGoogleAuthService googleAuthService;
		private readonly ApplicationDbContext context;
		private readonly IApplyInDayService applyService;

		/// <summary>
		/// 
		/// </summary>
		public ApplyInDayHandleController(IUsersService usersService, ICurrentUserService currentUserService, IUserActionServices userActionServices, IAuditStreamServices auditStreamServices, IGoogleAuthService googleAuthService, ApplicationDbContext context, IApplyInDayService applyService)
		{
			this.usersService = usersService;
			this.currentUserService = currentUserService;
			this.userActionServices = userActionServices;
			this.auditStreamServices = auditStreamServices;
			this.googleAuthService = googleAuthService;
			this.context = context;
			this.applyService = applyService;
		}

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
			var ua = userActionServices.Log(UserOperation.ModifyApply, id, $"保存", false, ActionRank.Infomation);
			try
			{
				CheckApplyModelAndDoTask(id, (x, u) =>
				{
					auditStreamServices.ModifyAuditStatus(ref x, AuditStatus.NotPublish, u);
					userActionServices.Status(ua, true, $"通过{u}");
					context.AppliesInday.Update(x);
					context.SaveChanges();
				});
			}
			catch (ActionStatusMessageException e)
			{
				userActionServices.Status(ua, false, e.Status.Message);
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
			var ua = userActionServices.Log(UserOperation.ModifyApply, id, $"发布", false, ActionRank.Infomation);
			EntitiesListViewModel<Guid> result = null;
			try
			{
				CheckApplyModelAndDoTask(id, (x, u) =>
				{
					var crashs = applyService.CheckIfHaveSameRangeVacation(x).Select(i => i.Id).ToList();
					if (crashs.Count > 0)
					{
						result = new EntitiesListViewModel<Guid>(crashs);
						return;
					}
					auditStreamServices.ModifyAuditStatus(ref x, AuditStatus.Auditing, u);
					userActionServices.Status(ua, true, $"通过{u}");
					context.AppliesInday.Update(x);
					context.SaveChanges();
				});
			}
			catch (ActionStatusMessageException e)
			{
				userActionServices.Status(ua, false, e.Status.Message);
				return new JsonResult(e.Status);
			}
			if (result != null) return new JsonResult(result);
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
			UserAction ua = userActionServices.Log(UserOperation.ModifyApply, id, $"撤回", false, ActionRank.Warning);
			try
			{
				CheckApplyModelAndDoTask(id, (x, u) =>
				{
					auditStreamServices.ModifyAuditStatus(ref x, AuditStatus.Withdrew, u);
					userActionServices.Status(ua, true, $"通过{u}");
					context.AppliesInday.Update(x);
					context.SaveChanges();
				});
			}
			catch (ActionStatusMessageException e)
			{
				userActionServices.Status(ua, false, e.Status.Message);
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
			UserAction ua = userActionServices.Log(DAL.Entities.UserInfo.UserOperation.ModifyApply, id, "作废休假", false, ActionRank.Danger);
			try
			{
				CheckApplyModelAndDoTask(id, (x, u) =>
				{
					userActionServices.Status(ua, false, $"通过{u}");
					auditStreamServices.ModifyAuditStatus(ref x, AuditStatus.Cancel, u);
					context.AppliesInday.Update(x);
					context.SaveChanges();
				}, false);  // 无需授权，因为ModifyAuditStatus已判断权限问题
			}
			catch (ActionStatusMessageException e)
			{
				userActionServices.Status(ua, false, e.Status.Message);
				return new JsonResult(e.Status);
			}
			userActionServices.Status(ua, true);
			return new JsonResult(ActionStatusMessage.Success);
		}

		private void CheckApplyModelAndDoTask(string id, Action<ApplyInday, string> callBack, bool needPermission = true)
		{
			_ = Guid.TryParse(id, out var gid);
			var apply = context.AppliesInday.FirstOrDefault(i => i.Id == gid) ?? throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.NotExist);
			var currentUser = currentUserService.CurrentUser ?? throw new ActionStatusMessageException(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			if (apply.BaseInfo.FromId != currentUser?.Id)
			{
				var permit = userActionServices.Permission(currentUser.Application.Permission, DictionaryAllPermission.Apply.Default, Operation.Update, currentUser.Id, apply.BaseInfo.CompanyCode, "执行休假申请的操作");
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
			var auditUser = currentUserService.CurrentUser;
			if (model.Auth?.AuthByUserID != null && auditUser?.Id != model.Auth?.AuthByUserID)
			{
				if (model.Auth.Verify(googleAuthService, currentUserService.CurrentUser?.Id))
					auditUser = usersService.GetById(model.Auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}
			try
			{
				var applyStrList = new StringBuilder();
				foreach (var a in model.Data.List) applyStrList.Append(a.Id).Append(':').Append(a.Action).Append(',');
				var ua = userActionServices.Log(DAL.Entities.UserInfo.UserOperation.AuditApply, auditUser.Id, $"授权审批申请:{applyStrList}", true, ActionRank.Warning);
				model.Data.List = model.Data.List.Distinct(new CompareAudit());
				var targets = model.Data.List.Select(d => d.Id);
				var raw_items = context.AppliesDb.Where(i => targets.Contains(i.Id));
				var items = model.ToAuditVDTO(auditUser, raw_items);
				var results = auditStreamServices.Audit(ref items);
				var result_list = items.List.Select(i => i.AuditItem.ToModel(raw_items.FirstOrDefault(a => a.Id == i.AuditItem.Id))).ToList();
				context.Applies.UpdateRange(result_list);
				context.SaveChanges();
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
