using System;
using System.Collections.Generic;
using BLL.Extensions;
using BLL.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DAL.DTO.Apply;
using Microsoft.AspNetCore.Authorization;
using TrainSchdule.ViewModels.Apply;
using DAL.DTO.Recall;
using TrainSchdule.ViewModels.System;
using DAL.Entities;
using BLL.Extensions.ApplyExtensions;
using TrainSchdule.ViewModels.Verify;
using Newtonsoft.Json;
using TrainSchdule.Extensions;
using DAL.QueryModel;
using TrainSchdule.ViewModels;
using DAL.Entities.ApplyInfo;
using BLL.Extensions.Common;
using System.Threading.Tasks;
using GoogleAuth;
using DAL.Entities.UserInfo;

namespace TrainSchdule.Controllers.Apply
{
	public partial class ApplyController
	{
		private void CheckValidQuery(QueryApplyViewModel model)
        {
			var auditUser = _currentUserService.CurrentUser;
			if (model.Auth?.AuthByUserID != null && model.Auth?.AuthByUserID != null && auditUser?.Id != model.Auth?.AuthByUserID)
			{
				if (model.Auth.Verify(_authService, _currentUserService.CurrentUser?.Id))
					auditUser = _usersService.GetById(model.Auth.AuthByUserID);
				else throw new ActionStatusMessageException(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}
			if (auditUser == null) throw new ActionStatusMessageException(auditUser.NotLogin());
			// 检查查询的单位范围，如果范围是空，则需要root权限
			var permitCompanies = model.CreateCompany?.Arrays ?? new List<string>() { "root" };
			foreach (var c in permitCompanies)
			{
				var permit = _userActionServices.Permission(auditUser?.Application?.Permission, DictionaryAllPermission.Apply.Default, Operation.Query, auditUser.Id, c, "审批列表");
				var cItem = _companiesService.GetById(c);
				if (!permit) throw new ActionStatusMessageException(new ApiResult(ActionStatusMessage.Account.Auth.Invalid.Default.Status, $"不具有{cItem?.Name}({c})的权限"));
			}
		}
		/// <summary>
		/// 条件快速查询
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		public IActionResult ListShadow([FromBody] QueryApplyViewModel model) {
			CheckValidQuery(model);
			var list = _applyService.QueryApplies(model, false, out var totalCount).Select(a => a.ToShadowDto());
			return new JsonResult(new EntitiesListViewModel<ApplyShadowDto>(list, totalCount));
		}
		/// <summary>
		/// 条件查询申请
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		public IActionResult List([FromBody] QueryApplyViewModel model)
		{
			CheckValidQuery(model);
			var list = _applyService.QueryApplies(model, false, out var totalCount).Select(a => a.ToSummaryDto());
            return new JsonResult(new EntitiesListViewModel<ApplySummaryDto>(list, totalCount));
		}
		/// <summary>
		/// 查询当前用户自己的申请
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult ListOfSelf(string id, int pageIndex = 0, int pageSize = 20, DateTime? start = null, DateTime? end = null)
		{
			var pages = new QueryByPage() { PageIndex = pageIndex, PageSize = pageSize };
			if (start == null) start = new DateTime(DateTime.Today.XjxtNow().Year, 1, 1);
			if (end == null) end = DateTime.Today.XjxtNow();
			end = end.Value.AddDays(1);

			var currentUser = _currentUserService.CurrentUser;
			var c = id == null ? currentUser : _usersService.GetById(id);
			var item = new { pages, id, start, end };
			var ua = _userActionServices.Log(UserOperation.AuditApply, c.Id, $"本人申请:{JsonConvert.SerializeObject(item)}", false, ActionRank.Infomation);

			if (id != null && id != currentUser.Id)
			{
				if (!_userActionServices.Permission(currentUser.Application.Permission, DictionaryAllPermission.Apply.Default, Operation.Query, currentUser.Id, c.CompanyInfo.Company.Code, $"{c.Id}的申请")) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			}
			var list = _context.AppliesDb.Where(a => a.BaseInfo.From.Id == c.Id).Where(a => a.Create >= start).Where(a => a.Create <= end);
			list = list.OrderByDescending(a => a.Create).ThenByDescending(a => a.Status);
			var result = list.SplitPage(pages);
			_userActionServices.Status(ua, true);
			return new JsonResult(new EntitiesListViewModel<ApplySummaryDto>(result.Item1.ToList()?.Select(a => a.ToSummaryDto()), result.Item2));
		}

		/// <summary>
		/// 查询当前用户可审批的申请
		/// </summary>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <param name="status">审批的状态，以##分割</param>
		/// <param name="actionStatus">我对此审批的状态</param>
		/// <param name="executeStatus"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult ListOfMyAudit(int pageIndex = 0, int pageSize = 20, string status = null, string actionStatus = null, string executeStatus = null)
		{
			var pages = new QueryByPage() { PageIndex = pageIndex, PageSize = pageSize };
			var c = _currentUserService.CurrentUser;
			var item = new { pages, status, actionStatus, executeStatus };
			var ua = _userActionServices.Log(UserOperation.AuditApply, c.Id, $"本人审批:{JsonConvert.SerializeObject(item)}", true, ActionRank.Infomation);

			var statusArr = status?.Split("##")?.Select(i => Convert.ToInt32(i));
			var r = _context.AppliesDb;//.Where(a => a.NowAuditStep.MembersFitToAudit.Contains(c.Id));
			if (statusArr != null && statusArr.Any()) r = r.Where(a => statusArr.Contains((int)a.Status)); // 查出所有状态符合的
			r = r.Where(a => a.ApplyAllAuditStep.Any(s => s.MembersFitToAudit.Contains(c.Id)));// 查出所有涉及本人的
			if (executeStatus != null)
			{
				int.TryParse(executeStatus, out var executeStatusInt);
				r = r.Where(a => (int)a.ExecuteStatus == executeStatusInt);
			}
			if (actionStatus != null)
			{
				switch (actionStatus.ToLower())
				{
					case "accept":
						{
							r = r.Where(a => a.Response.Any(res => res.AuditingBy.Id == c.Id && res.Status == Auditing.Accept));
							break;
						}
					case "deny":
						{
							r = r.Where(a => a.Response.Any(res => res.AuditingBy.Id == c.Id && res.Status == Auditing.Denied));
							break;
						}
					case "unreceive":
						{
							r = r.Where(a => a.Status == AuditStatus.AcceptAndWaitAdmin || a.Status == AuditStatus.Auditing); // 当前处于审批中的
							r = r.Where(a => a.Response.All(res => res.AuditingBy.Id != c.Id)); // 我没有进行审批的
							r = r.Where(a => a.NowAuditStep != null).Where(a => !a.NowAuditStep.MembersFitToAudit.Contains(c.Id)); // 并且当前页不该我审批的
							break;
						}
					case "received":
						{
							r = r.Where(a => a.Status == AuditStatus.AcceptAndWaitAdmin || a.Status == AuditStatus.Auditing); // 当前处于审批中的
							r = r.Where(a => a.NowAuditStep != null).Where(a => a.NowAuditStep.MembersFitToAudit.Contains(c.Id)); // 并且当前页该我审批的
							r = r.Where(a => !a.NowAuditStep.MembersAcceptToAudit.Contains(c.Id)); // 并且当前我没有审核的
							break;
						}
					default: return new JsonResult(ActionStatusMessage.ApplyMessage.Operation.Default);
				}
			}
			//r = r.Where(a => !a.NowAuditStep.MembersAcceptToAudit.Contains(c.Id));
			var list = r.OrderByDescending(a => a.Create).ThenByDescending(a => a.Status);
			var result = list.SplitPage(pages);
			var f_result = new EntitiesListDataModel<ApplySummaryDto>(result.Item1.ToList()?.Select(a => a.ToSummaryDto()), result.Item2);
			return new JsonResult(f_result);
		}

		/// <summary>
		/// 获取申请的详情
		/// </summary>
		/// <param name="id">申请的id</param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		public IActionResult Detail(string id)
		{
			Guid.TryParse(id, out var aId);
			var apply = _applyService.GetById(aId);
			if (apply == null) return new JsonResult(ActionStatusMessage.ApplyMessage.NotExist);
			apply.ApplyAllAuditStep = apply.ApplyAllAuditStep.OrderBy(s => s.Index);
			return new JsonResult(new InfoApplyDetailViewModel()
			{
				Data = apply.ToDetaiDto(_usersService.VacationInfo(apply.BaseInfo.From, apply.RequestInfo.StampLeave?.Year??DateTime.Now.XjxtNow().Year), _context)
			});
		}

		/// <summary>
		/// 恢复被删除的申请
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult RestoreApply([FromForm] ApplyRestoreViewModel model)
		{
			var apply = _context.Applies.Where(a => a.Id == model.Id).FirstOrDefault();
			if (apply == null) return new JsonResult(ActionStatusMessage.ApplyMessage.NotExist);
			var auditUser = _currentUserService.CurrentUser;
			if (model.Auth?.AuthByUserID != null && model.Auth?.AuthByUserID != null && auditUser?.Id != model.Auth?.AuthByUserID)
			{
				if (model.Auth.Verify(_authService, _currentUserService.CurrentUser?.Id))
					auditUser = _usersService.GetById(model.Auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}
			var permit = _userActionServices.Permission(auditUser?.Application?.Permission, DictionaryAllPermission.Apply.Default, Operation.Query, auditUser.Id, "root");
			if (!permit) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			apply.IsRemoved = false;
			_context.Applies.Update(apply);
			_context.SaveChanges();
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 查询所有已删除的申请
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult RomovedApply(int pageIndex = 0, int pageSize = 20)
		{
			var list = _context.Applies.Where(a => a.IsRemoved).OrderByDescending(a => a.IsRemovedDate);
			var result = list.SplitPage<DAL.Entities.ApplyInfo.Apply>(pageIndex, pageSize);
			return new JsonResult(new EntitiesListViewModel<ApplySummaryDto>(result.Item1.ToList().Select(c => c.ToSummaryDto()),result.Item2));
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		public IActionResult RemoveAllUnSaveApply()
		{
			var result = _applyService.RemoveAllUnSaveApply(TimeSpan.FromDays(1));
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 召回休假
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		public IActionResult RecallOrder([FromBody] RecallCreateViewModel model)
		{
			var authUser = model.Auth.AuthUser(_authService, _currentUserService.CurrentUser?.Id);
			var ua = _userActionServices.Log(DAL.Entities.UserInfo.UserOperation.ModifyApply, authUser, $"召回{model.Data.Apply}");
			if (authUser != model.Data.HandleBy) return new JsonResult(model.Auth.PermitDenied());

			var recall = model.Data.ToVDto<RecallOrderVDto>();
			var result = recallOrderServices.Create(recall);
			_userActionServices.Status(ua, true);
			return new JsonResult(new APIResponseIdViewModel(result.Id, ActionStatusMessage.Success));
		}

		/// <summary>
		/// 通过召回id获取召回信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		public IActionResult RecallOrder(Guid id)
		{
			var recall = _context.RecallOrders.Where(r => r.Id == id).FirstOrDefault();
			if (recall == null) return new JsonResult(ActionStatusMessage.ApplyMessage.RecallMessage.NotExist);
			var apply = _context.AppliesDb.Where(a => a.RecallId == id).FirstOrDefault();
			return new JsonResult(new EntityDirectViewModel<HandleByVdto>(recall.ToVDto(apply)));
		}

		/// <summary>
		/// 确认休假执行情况
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		public IActionResult ExecuteStatus([FromBody] RecallCreateViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			var authUser = model.Auth.AuthUser(_authService, _usersService, _currentUserService.CurrentUser?.Id);
			if (authUser.Id != model.Data.HandleBy) return new JsonResult(model.Auth.PermitDenied());

			var m = model.Data.ToVDto<ExecuteStatusVDto>();
			var apply = _applyService.GetById(m.Apply);
			var targetUser = apply.BaseInfo.From;
			var permit = _userActionServices.Permission(authUser.Application.Permission, DictionaryAllPermission.Apply.Default, Operation.Update, authUser.Id, targetUser.CompanyInfo.Company.Code, $"确认{targetUser.Id}归队时间");
			if (!permit) return new JsonResult(model.Auth.PermitDenied());
			var result = recallOrderServices.Create(apply, m);
			return new JsonResult(new APIResponseIdViewModel(result.Id, ActionStatusMessage.Success));
		}

		/// <summary>
		/// 通过休假执行情况id获取详情
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		public IActionResult ExecuteStatus(Guid id)
		{
			var model = _context.ApplyExcuteStatus.Where(r => r.Id == id).FirstOrDefault();
			if (model == null) return new JsonResult(ActionStatusMessage.ApplyMessage.RecallMessage.ExecuteNotExist);
			var apply = _context.AppliesDb.Where(a => a.ExecuteStatusDetailId == id).FirstOrDefault();
			return new JsonResult(new EntityDirectViewModel<HandleByVdto>(model.ToVDto(apply)));
		}
	}
}