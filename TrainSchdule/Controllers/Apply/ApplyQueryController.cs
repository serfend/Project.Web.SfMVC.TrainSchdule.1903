using BLL.Extensions;
using BLL.Extensions.ApplyExtensions;
using BLL.Extensions.Common;
using BLL.Helpers;
using DAL.DTO.Apply;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.ApplyInfo.DailyApply;
using DAL.Entities.UserInfo;
using DAL.QueryModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Apply;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers.Apply
{

	public partial class ApplyController
	{

		private void CheckValidQuery(QueryApplyViewModel model)
		{
			var auditUser = currentUserService.CurrentUser;
			if (model.Auth?.AuthByUserID != null && model.Auth?.AuthByUserID != null && auditUser?.Id != model.Auth?.AuthByUserID)
			{
				if (model.Auth.Verify(authService, currentUserService.CurrentUser?.Id))
					auditUser = usersService.GetById(model.Auth.AuthByUserID);
				else throw new ActionStatusMessageException(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}
			if (auditUser == null) throw new ActionStatusMessageException(auditUser.NotLogin());
			// 检查查询的单位范围，如果范围是空，则需要root权限
			var permitCompanies = model.CreateCompany?.Arrays ?? new List<string>() { "root" };
			foreach (var c in permitCompanies)
			{
				var permit = userActionServices.Permission(auditUser?.Application?.Permission, DictionaryAllPermission.Apply.Default, Operation.Query, auditUser.Id, c, "审批列表");
				var cItem = companiesService.GetById(c);
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
		public IActionResult ListShadow([FromBody] QueryApplyViewModel model)
		{
			CheckValidQuery(model);
			var list = applyService.QueryApplies(model, false, out var totalCount).Select(a => a.ToShadowDto());
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
			var list = applyService.QueryApplies(model, false, out var totalCount).Select(a => a.ToSummaryDto(a.RequestInfo));
			return new JsonResult(new EntitiesListViewModel<ApplySummaryDto<ApplyRequest>>(list, totalCount));
		}
		/// <summary>
		/// 查询当前用户自己的申请
		/// </summary>
		/// <returns></returns>
		[Route("{entityType}")]
		[HttpGet]
		public IActionResult ListOfSelf(string id, int pageIndex = 0, int pageSize = 20, int? MainStatus = null, DateTime? start = null, DateTime? end = null, string entityType = null)
		{
			var pages = new QueryByPage() { PageIndex = pageIndex, PageSize = pageSize };
			if (start == null) start = new DateTime(DateTime.Today.XjxtNow().Year, 1, 1);
			if (end == null) end = DateTime.Today.XjxtNow();
			end = end.Value.AddDays(1);

			var currentUser = currentUserService.CurrentUser;
			var c = id == null ? currentUser : usersService.GetById(id);
			if (c == null) return new JsonResult(c.NotExist());
			var item = new { pages, id, start, end };
			var ua = userActionServices.Log(UserOperation.AuditApply, c.Id, $"{entityType}-本人申请:{JsonConvert.SerializeObject(item)}", false, ActionRank.Infomation);

			if (id != null && id != currentUser.Id)
			{
				if (!userActionServices.Permission(currentUser.Application.Permission, DictionaryAllPermission.Apply.Default, Operation.Query, currentUser.Id, c.CompanyInfo.CompanyCode, $"{c.Id}的申请")) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			}
			if (entityType == "vacation")
			{
				var list = context.AppliesDb
				.Where(a => a.BaseInfo.FromId == c.Id)
				.Where(a => a.RequestInfo.StampLeave >= start)
				.Where(a => a.RequestInfo.StampLeave <= end);
				if (MainStatus != null) list = list.Where(a => (int)a.MainStatus == MainStatus);
				list = list.OrderByDescending(a => a.RequestInfo.StampLeave).ThenByDescending(a => a.Status);
				var result = list.SplitPage(pages);
				userActionServices.Status(ua, true);
				return new JsonResult(new EntitiesListViewModel<ApplySummaryDto<ApplyRequest>>(result.Item1.ToList()?.Select(a => a.ToSummaryDto(a.RequestInfo)), result.Item2));
			}
			else
			{
				var list = context.AppliesIndayDb
								.Where(a => a.BaseInfo.FromId == c.Id)
								.Where(a => a.RequestInfo.StampLeave >= start)
								.Where(a => a.RequestInfo.StampLeave <= end);
				if (MainStatus != null) list = list.Where(a => (int)a.MainStatus == MainStatus);
				list = list.OrderByDescending(a => a.RequestInfo.StampLeave).ThenByDescending(a => a.Status);
				var result = list.SplitPage(pages);
				userActionServices.Status(ua, true);
				return new JsonResult(new EntitiesListViewModel<ApplySummaryDto<ApplyIndayRequest>>(result.Item1.ToList()?.Select(a => a.ToSummaryDto(a.RequestInfo)), result.Item2));
			}

		}
        /// <summary>
        /// 查询当前用户可审批的申请
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="status">审批的状态，以##分割</param>
        /// <param name="MainStatus">申请的主要状态，可选项</param>
        /// <param name="actionStatus">我对此审批的状态</param>
        /// <param name="executeStatus"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        [HttpGet]
		[Route("{entityType}")]
		public IActionResult ListOfMyAudit(int pageIndex = 0, int pageSize = 20, string status = null, int? MainStatus = null, string actionStatus = null, string executeStatus = null,string entityType = null)
		{
			var pages = new QueryByPage() { PageIndex = pageIndex, PageSize = pageSize };
			var c = currentUserService.CurrentUser;
			var item = new { pages, status, actionStatus, executeStatus };
			var ua = userActionServices.Log(UserOperation.AuditApply, c.Id, $"本人审批:{JsonConvert.SerializeObject(item)}", true, ActionRank.Infomation);

			var statusArr = status?.Split("##")?.Select(i => Convert.ToInt32(i));
			IQueryable<IAppliable> r = entityType=="vacation"? context.AppliesDb:context.AppliesIndayDb;//.Where(a => a.NowAuditStep.MembersFitToAudit.Contains(c.Id));
			if (statusArr != null && statusArr.Any()) r = r.Where(a => statusArr.Contains((int)a.Status)); // 查出所有状态符合的
			if (MainStatus != null) r = r.Where(a => (int)a.MainStatus == MainStatus);
			r = r.Where(a => a.ApplyAllAuditStep.Any(s => s.MembersFitToAudit.Contains(c.Id)));// 查出所有涉及本人的
			if (executeStatus != null)
			{
				_ = int.TryParse(executeStatus, out var executeStatusInt);
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
            var ids = result.Item1.Select(i => i.Id);
            if (entityType == "vacation")
            {
				var result_items = result.Item1.ToList()?.Select(a => a.ToSummaryDto(((DAL.Entities.ApplyInfo.Apply)a).RequestInfo));
				var f_result = new EntitiesListDataModel<ApplySummaryDto<ApplyRequest>>(result_items, result.Item2);
				return new JsonResult(f_result);
            }
            else
            {
				var result_items = result.Item1.ToList()?.Select(a => a.ToSummaryDto(((ApplyInday)a).RequestInfo));
				var f_result = new EntitiesListDataModel<ApplySummaryDto<ApplyIndayRequest>>(result_items, result.Item2);
				return new JsonResult(f_result);
			}

		}

		/// <summary>
		/// 获取申请的详情
		/// </summary>
		/// <param name="id">申请的id</param>
		/// <param name="entityType"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("{entityType}")]
		[AllowAnonymous]
		public IActionResult Detail(string id, string entityType)
		{
			Guid.TryParse(id, out var aId);
			if (entityType == "vacation")
			{
				var apply = applyService.GetById(aId);
				if (apply == null) return new JsonResult(ActionStatusMessage.ApplyMessage.NotExist);
				apply.ApplyAllAuditStep = apply.ApplyAllAuditStep.OrderBy(s => s.Index);
				var vacationInfoDto = usersService.VacationInfo(apply.BaseInfo.From, apply.RequestInfo.StampLeave?.Year ?? DateTime.Now.XjxtNow().Year, apply.MainStatus);
				return new JsonResult(new EntityViewModel<ApplyDetailDto<ApplyRequest>>(apply.ToDetaiDto(vacationInfoDto, apply.RequestInfo, context)));
			}
			else
			{
				var apply = applyInDayService.GetById(aId);
				if (apply == null) return new JsonResult(ActionStatusMessage.ApplyMessage.NotExist);
				apply.ApplyAllAuditStep = apply.ApplyAllAuditStep.OrderBy(s => s.Index);
				return new JsonResult(new EntityViewModel<ApplyDetailDto<ApplyIndayRequest>>(apply.ToDetaiDto(null, apply.RequestInfo, context)));
			}
		}

	}
}
