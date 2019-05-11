﻿using System;
using System.Collections.Generic;
using BLL.Extensions;
using BLL.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DAL.DTO.Apply;
using Microsoft.AspNetCore.Authorization;
using TrainSchdule.ViewModels.Apply;

namespace TrainSchdule.Controllers.Apply
{
	public partial class ApplyController
	{
		/// <summary>
		/// 来自此用户发布的
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesDefaultResponseType(typeof(IEnumerable<ApplySummaryDto>))]

		public IActionResult FromUser(string id)
		{
			id = id ?? _currentUserService.CurrentUser?.Id;
			if(id==null)return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			var targetUser = _usersService.Get(id);
			if(targetUser==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			var list = _applyService.GetApplyBySubmitUser(id);
			return new JsonResult(new ApplyListViewModel()
			{
				Data = new ApplyListDataModel()
				{
					List = list.Select(a=>a.ToSummaryDto())
				}
			});
		}
		/// <summary>
		/// 交给此用户审批的（选定单位）
		/// </summary>
		/// <param name="id"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesDefaultResponseType(typeof(IEnumerable<ApplySummaryDto>))]
		public IActionResult ToUser(string id,string code)
		{
			id = id ?? _currentUserService.CurrentUser?.Id;
			if(id==null) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			var targetUser = _usersService.Get(id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var targetCompany = _companiesService.Get(code);
			if(targetCompany==null)return new JsonResult(ActionStatusMessage.Company.NotExist);
			if(!_companiesService.CheckManagers(code, id))return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default) ;
			return GetCompanyApply(code);
		}
		/// <summary>
		/// 来自此单位的
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesDefaultResponseType(typeof(IEnumerable<ApplySummaryDto>))]

		public IActionResult FromCompany(string code)
		{
			code = code ?? _currentUserService.CurrentUser?.CompanyInfo?.Company?.Code;
			if(code==null) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			var targetCompany = _companiesService.Get(code);
			if (targetCompany == null) return new JsonResult(ActionStatusMessage.Company.NotExist);
			return new JsonResult(new ApplyListViewModel()
			{
				Data = new ApplyListDataModel()
				{
					List = _applyService.GetApplyBySubmitCompany(code).Select(a=>a.ToSummaryDto())
				}
			});
		}
		/// <summary>
		/// 交给此单位审批的
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesDefaultResponseType(typeof(IEnumerable<ApplySummaryDto>))]

		public IActionResult ToCompany(string code)
		{
			code = code ?? _currentUserService.CurrentUser?.CompanyInfo?.Company?.Code;
			var targetCompany = _companiesService.Get(code);
			if (targetCompany == null) return new JsonResult(ActionStatusMessage.Company.NotExist);
			return GetCompanyApply(targetCompany.Code);
		}
		private IActionResult GetCompanyApply(string code)
		{
			var list = _applyService.GetApplyByToAuditCompany(code);
			return new JsonResult(new ApplyListViewModel()
			{
				Data = new ApplyListDataModel()
				{
					List = list.Select(a => a.ToSummaryDto())
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
			var apply = _applyService.Get(aId);
			if (apply == null) return new JsonResult(ActionStatusMessage.Apply.NotExist);
			return new JsonResult(new InfoApplyDetailViewModel()
			{
				Data = apply.ToDetaiDto()
			});
		}

	}
}
