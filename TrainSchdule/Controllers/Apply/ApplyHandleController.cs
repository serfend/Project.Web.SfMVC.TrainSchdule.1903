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
		public IActionResult List([FromBody]QueryApplyDataModel model)
		{
			try
			{
				if (model == null) return new JsonResult(ActionStatusMessage.Apply.Default);

				var currentUser = _currentUserService.CurrentUser;
				var list = _applyService.QueryApplies(model, false, out var totalCount)?.Select(a => a.ToSummaryDto());
				return new JsonResult(new ApplyListViewModel()
				{
					Data = new ApplyListDataModel()
					{
						List = list,
						TotalCount = totalCount
					}
				}); ;
			}
			catch (ActionStatusMessageException ex)
			{
				return new JsonResult(new ApiResult(-1, ex.Message));
			}
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
			var managedCompany = _usersService.InMyManage(currentUser, out var totalCount);
			var userPermitCompany = managedCompany.Any<Company>(c => c.Code == apply.Response.NowAuditCompany()?.Code);
			return new JsonResult(new InfoApplyDetailViewModel()
			{
				Data = apply.ToDetaiDto(_usersService.VocationInfo(apply.BaseInfo.From), userPermitCompany)
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
			var apply = _context.Applies.Where(a => a.RecallId == id).FirstOrDefault();
			return new JsonResult(new RecallViewModel()
			{
				Data = recall.ToVDto(apply)
			});
		}
	}
}
