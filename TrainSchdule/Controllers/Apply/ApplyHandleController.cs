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
using DAL.Entities.ApplyInfo.DailyApply;
using DAL.Entities.Permisstions;
using Abp.Extensions;

namespace TrainSchdule.Controllers.Apply
{
	public partial class ApplyController
	{
		/// <summary>
		/// 恢复被删除的申请
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult RestoreApply([FromForm] ApplyRestoreViewModel model)
		{
			var apply = context.Applies.Where(a => a.Id == model.Id).FirstOrDefault();
			if (apply == null) return new JsonResult(ActionStatusMessage.ApplyMessage.NotExist);
			var auditUser = currentUserService.CurrentUser;
			if (model.Auth?.AuthByUserID.IsNullOrEmpty() == false && model.Auth?.AuthByUserID.IsNullOrEmpty() == false && auditUser?.Id != model.Auth?.AuthByUserID)
			{
				if (model.Auth.Verify(authService, currentUserService.CurrentUser?.Id))
					auditUser = usersService.GetById(model.Auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}
			var permit = userActionServices.Permission(auditUser, ApplicationPermissions.Apply.Vacation.Detail.Item, PermissionType.Read, "root","恢复已删除的申请");
			if (!permit) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			apply.IsRemoved = false;
			context.Applies.Update(apply);
			context.SaveChanges();
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 查询所有已删除的申请
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult RomovedApply(int pageIndex = 0, int pageSize = 20)
		{
			var list = context.Applies.Where(a => a.IsRemoved).OrderByDescending(a => a.IsRemovedDate);
			var result = list.SplitPage<DAL.Entities.ApplyInfo.Apply>(pageIndex, pageSize);
			return new JsonResult(new EntitiesListViewModel<ApplySummaryDto<ApplyRequest>>(result.Item1.ToList().Select(c => c.ToSummaryDto(c.RequestInfo)),result.Item2));
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		public IActionResult RemoveAllUnSaveApply()
		{
			var result = applyService.RemoveAllUnSaveApply(TimeSpan.FromDays(1));
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
			var authUser = model.Auth.AuthUser(authService, currentUserService.CurrentUser?.Id);
			var ua = userActionServices.Log(DAL.Entities.UserInfo.UserOperation.ModifyApply, authUser, $"召回{model.Data.Apply}");
			if (authUser != model.Data.HandleBy) return new JsonResult(model.Auth.PermitDenied());
			var recall = model.Data.ToVDto<RecallOrderVDto>();
			var result = recallOrderServices.Create(recall);
			userActionServices.Status(ua, true);
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
			var recall = context.RecallOrders.Where(r => r.Id == id).FirstOrDefault();
			if (recall == null) return new JsonResult(ActionStatusMessage.ApplyMessage.RecallMessage.NotExist);
			var apply = context.AppliesDb.Where(a => a.RecallId == id).FirstOrDefault();
			return new JsonResult(new EntityDirectViewModel<HandleByVdto>(recall.ToVDto(apply.Id)));
		}

        /// <summary>
        /// 确认休假执行情况
        /// </summary>
        /// <param name="model"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        [HttpPost]
		[Route("{entityType}")]
		[AllowAnonymous]
		public IActionResult ExecuteStatus([FromBody] RecallCreateViewModel model,string entityType)
		{
			var authUser = model.Auth.AuthUser(authService, usersService, currentUserService.CurrentUser?.Id);
			if (authUser.Id != model.Data.HandleBy) return new JsonResult(model.Auth.PermitDenied());
			var m = model.Data.ToVDto<ExecuteStatusVDto>();
            if (entityType == "vacation")
            {
				var apply = applyService.GetById(m.Apply);
				var targetUser = apply.BaseInfo.From;
				var permit = userActionServices.Permission(authUser, ApplicationPermissions.Apply.Vacation.ExecuteStatus.Item, PermissionType.Write, targetUser.CompanyInfo.CompanyCode, $"确认{targetUser.Id}归队时间");
				if (!permit) return new JsonResult(model.Auth.PermitDenied());
				var result = recallOrderServices.Create(apply.RequestInfo, apply.ExecuteStatus, m,false);
				apply.ExecuteStatus = result.Item1;
				apply.ExecuteStatusDetailId = result.Item2.Id;
				context.Applies.Update(apply);
				context.SaveChanges();
				return new JsonResult(new APIResponseIdViewModel(result.Item2.Id, ActionStatusMessage.Success));
            }
            else
            {
				var apply = applyInDayService.GetById(m.Apply);
				var targetUser = apply.BaseInfo.From;
				var permit = userActionServices.Permission(authUser, ApplicationPermissions.Apply.ApplyInday.ExecuteStatus.Item, PermissionType.Write,targetUser.CompanyInfo.CompanyCode, $"确认{targetUser.Id}归队时间");
				if (!permit) return new JsonResult(model.Auth.PermitDenied());
				var result = recallOrderServices.Create(apply.RequestInfo, apply.ExecuteStatus, m, true) ;
				apply.ExecuteStatus = result.Item1;
				apply.ExecuteStatusDetailId = result.Item2.Id;
				context.AppliesInday.Update(apply);
				context.SaveChanges();
				return new JsonResult(new APIResponseIdViewModel(result.Item2.Id, ActionStatusMessage.Success));
			}
		}

        /// <summary>
        /// 通过休假执行情况id获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        [HttpGet]
		[Route("{entityType}")]
		[AllowAnonymous]
		public IActionResult ExecuteStatus(Guid id,string entityType)
		{
			var model = context.ApplyExcuteStatus.Where(r => r.Id == id).FirstOrDefault();
			if (model == null) return new JsonResult(ActionStatusMessage.ApplyMessage.RecallMessage.ExecuteNotExist);
            if (entityType == "vacation")
            {
				var apply = context.AppliesDb.Where(a => a.ExecuteStatusDetailId == id).FirstOrDefault();
				return new JsonResult(new EntityDirectViewModel<HandleByVdto>(model.ToVDto(apply.Id)));
            }
            else
            {
				var apply = context.AppliesInday.Where(a => a.ExecuteStatusDetailId == id).FirstOrDefault();
				return new JsonResult(new EntityDirectViewModel<HandleByVdto>(model.ToVDto(apply.Id)));
			}
		}
	}
}