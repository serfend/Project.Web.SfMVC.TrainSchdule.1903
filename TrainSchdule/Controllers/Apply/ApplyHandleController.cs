﻿using System;
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

namespace TrainSchdule.Controllers.Apply
{
	public partial class ApplyController
	{
		/// <summary>
		/// 条件查询申请
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		public IActionResult List([FromBody]QueryApplyViewModel model)
		{
			try
			{
				if (model == null) return new JsonResult(ActionStatusMessage.Apply.Default);

				if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
				var auditUser = _currentUserService.CurrentUser;
				if (model.Auth?.AuthByUserID != null && model.Auth?.AuthByUserID != null && auditUser?.Id != model.Auth?.AuthByUserID)
				{
					if (model.Auth.Verify(_authService, _currentUserService.CurrentUser?.Id))
						auditUser = _usersService.Get(model.Auth.AuthByUserID);
					else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
				}

				// 检查查询的单位范围，如果范围是空，则需要root权限
				var permitCompaines = model.CreateCompany?.Value ?? "Root";

				var permit = _userActionServices.Permission(auditUser?.Application?.Permission, DictionaryAllPermission.Apply.Default, Operation.Query, auditUser.Id, permitCompaines);
				if (!permit) return new JsonResult(new ApiResult(ActionStatusMessage.Account.Auth.Invalid.Default.Status, $"不具有{permitCompaines}的权限"));
				var list = _applyService.QueryApplies(model, false, out var totalCount)?.Select(a => a.ToSummaryDto());
				return new JsonResult(new ApplyListViewModel()
				{
					Data = new ApplyListDataModel()
					{
						List = list,
						TotalCount = totalCount
					}
				});
			}
			catch (ActionStatusMessageException ex)
			{
				return new JsonResult(new ApiResult(-1, ex.Message));
			}
		}

		/// <summary>
		/// 查询当前用户自己的申请
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult ListOfSelf(int pageIndex = 0, int pageSize = 20)
		{
			var c = _currentUserService.CurrentUser;
			var list = _context.AppliesDb.Where(a => a.BaseInfo.From.Id == c.Id).OrderByDescending(a => a.Create).ThenBy(a => a.Status).Skip(pageIndex * pageSize).Take(pageSize).ToList();
			return new JsonResult(new ApplyListViewModel()
			{
				Data = new ApplyListDataModel()
				{
					List = list?.Select(a => a.ToSummaryDto())
				}
			});
		}

		/// <summary>
		/// 查询当前用户可审批的申请
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult ListOfMyAudit(int pageIndex = 0, int pageSize = 20)
		{
			var c = _currentUserService.CurrentUser;
			//.Where(a => a.ApplyAllAuditStep.Any(s => s.MembersFitToAudit.Contains(c.Id)))
			var r = _context.AppliesDb.Where(a => a.NowAuditStep.MembersFitToAudit.Contains(c.Id));
			r = r.Where(a => !a.NowAuditStep.MembersAcceptToAudit.Contains(c.Id));
			var list = r.OrderByDescending(a => a.Status).ThenBy(a => a.Create).Skip(pageIndex * pageSize).Take(pageSize).ToList();
			return new JsonResult(new ApplyListViewModel()
			{
				Data = new ApplyListDataModel()
				{
					List = list?.Select(a => a.ToSummaryDto())
				}
			});
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
			if (apply == null) return new JsonResult(ActionStatusMessage.Apply.NotExist);
			var currentUser = _currentUserService.CurrentUser;
			return new JsonResult(new InfoApplyDetailViewModel()
			{
				Data = apply.ToDetaiDto(_usersService.VocationInfo(apply.BaseInfo.From))
			});
		}

		/// <summary>
		/// 恢复被删除的申请
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult RestoreApply([FromForm]ApplyRestoreViewModel model)
		{
			var apply = _context.Applies.Where(a => a.Id == model.Id).FirstOrDefault();
			if (apply == null) return new JsonResult(ActionStatusMessage.Apply.NotExist);
			var auditUser = _currentUserService.CurrentUser;
			if (model.Auth?.AuthByUserID != null && model.Auth?.AuthByUserID != null && auditUser?.Id != model.Auth?.AuthByUserID)
			{
				if (model.Auth.Verify(_authService, _currentUserService.CurrentUser?.Id))
					auditUser = _usersService.Get(model.Auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}
			var permit = _userActionServices.Permission(auditUser?.Application?.Permission, DictionaryAllPermission.Apply.Default, Operation.Query, auditUser.Id, "Root");
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
		public IActionResult RomovedApply()
		{
			var list = _context.Applies.Where(a => a.IsRemoved).ToList();
			return new JsonResult(new ApplyListViewModel()
			{
				Data = new ApplyListDataModel()
				{
					List = list?.Select(a => a.ToSummaryDto())
				}
			});
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		public IActionResult RemoveAllUnSaveApply()
		{
			_applyService.RemoveAllUnSaveApply();
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 召回休假
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		public IActionResult RecallOrder([FromBody]RecallCreateViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));

			if (!model.Auth.Verify(_authService, _currentUserService.CurrentUser?.Id)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			RecallOrder result;
			try
			{
				var recall = model.Data.ToVDto();
				result = recallOrderServices.Create(recall);
			}
			catch (ActionStatusMessageException ex)
			{
				return new JsonResult(ex.Status);
			}
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
			if (recall == null) return new JsonResult(ActionStatusMessage.Apply.Recall.NotExist);
			var apply = _context.AppliesDb.Where(a => a.RecallId == id).FirstOrDefault();
			return new JsonResult(new RecallViewModel()
			{
				Data = recall.ToVDto(apply)
			});
		}
	}
}