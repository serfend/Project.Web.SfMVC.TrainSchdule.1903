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

namespace TrainSchdule.Controllers.Apply
{
	public partial class ApplyController
	{
		/// <summary>
		/// 来自此用户发布的
		/// </summary>
		/// <param name="id"></param>
		/// <param name="page"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesDefaultResponseType(typeof(IEnumerable<ApplySummaryDto>))]

		public IActionResult FromUser(string id,int page,int pageSize)
		{
			id = id ?? _currentUserService.CurrentUser?.Id;
			if(id==null)return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			var targetUser = _usersService.Get(id);
			if(targetUser==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			var list = _applyService.GetApplyBySubmitUser(id, page,pageSize);
			return new JsonResult(new ApplyListViewModel()
			{
				Data = new ApplyListDataModel()
				{
					List = list.Select(a=>a.ToSummaryDto(false))
				}
			});
		}

		/// <summary>
		/// 交给此用户审批的（选定单位）
		/// </summary>
		/// <param name="id"></param>
		/// <param name="code"></param>
		/// <param name="page"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesDefaultResponseType(typeof(IEnumerable<ApplySummaryDto>))]
		public IActionResult ToUser(string id,string code,int page,int pageSize)
		{
			id = id ?? _currentUserService.CurrentUser?.Id;
			if(id==null) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			var targetUser = _usersService.Get(id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var currentUser = _currentUserService.CurrentUser;
			if (currentUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var companyManages = _usersService.InMyManage(_currentUserService.CurrentUser.Id);
			var canAuditNot = companyManages.Any<Company>(c => c.Code == currentUser.CompanyInfo.Company.Code);
			var targetCompany = _companiesService.Get(code);
			if(targetCompany==null)return new JsonResult(ActionStatusMessage.Company.NotExist);
			if(!_companiesService.CheckManagers(code, id))return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default) ;
			return GetCompanyApply(code,canAuditNot,page,pageSize);
		}

		/// <summary>
		/// 来自此单位的
		/// </summary>
		/// <param name="code"></param>
		/// <param name="page"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesDefaultResponseType(typeof(IEnumerable<ApplySummaryDto>))]

		public IActionResult FromCompany(string code,int page,int pageSize)
		{
			var currentUserCompany = _currentUserService.CurrentUser?.CompanyInfo?.Company;
			if (currentUserCompany == null) return new JsonResult(ActionStatusMessage.Company.NoneCompanyBelong);
			code = code ?? currentUserCompany.Code;
			if(code==null) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			var targetCompany = _companiesService.Get(code);
			if (targetCompany == null) return new JsonResult(ActionStatusMessage.Company.NotExist);
			return new JsonResult(new ApplyListViewModel()
			{
				Data = new ApplyListDataModel()
				{
					List = _applyService.GetApplyBySubmitCompany(code,page,pageSize).Select(a=>a.ToSummaryDto(false))
				}
			});
		}

		/// <summary>
		/// 交给此单位审批的
		/// </summary>
		/// <param name="code"></param>
		/// <param name="page"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesDefaultResponseType(typeof(IEnumerable<ApplySummaryDto>))]

		public IActionResult ToCompany(string code,int page,int pageSize)
		{
			code = code ?? _currentUserService.CurrentUser?.CompanyInfo?.Company?.Code;
			var targetCompany = _companiesService.Get(code);
			if (targetCompany == null) return new JsonResult(ActionStatusMessage.Company.NotExist);
			var currentUser = _currentUserService.CurrentUser;
			if (currentUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var companyManages = _usersService.InMyManage(_currentUserService.CurrentUser.Id);
			var canAuditNow = companyManages.Any<Company>(c=>c.Code==currentUser.CompanyInfo.Company.Code);
			return GetCompanyApply(targetCompany.Code,canAuditNow,page,pageSize);
		}
		private IActionResult GetCompanyApply(string code,bool canAuditNow, int page,int pageSize)
		{
			var list = _applyService.GetApplyByToAuditCompany(code, page, pageSize);
			return new JsonResult(new ApplyListViewModel()
			{
				Data = new ApplyListDataModel()
				{
					List = list.Select(a => a.ToSummaryDto(canAuditNow))
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
			var managedCompany = _usersService.InMyManage(_currentUserService.CurrentUser.Id);
			var userPermitCompany = managedCompany.Any<Company>(c=>c.Code==apply.Response.NowAuditCompany().Code);
			return new JsonResult(new InfoApplyDetailViewModel()
			{
				Data = apply.ToDetaiDto(userPermitCompany)
			});
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
			if(!ModelState.IsValid) return new JsonResult(new Status(ActionStatusMessage.Fail.status, JsonConvert.SerializeObject(ModelState.AllModelStateErrors())));
			if (!model.Auth.Verify(_authService))return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			RecallOrder result;
			try
			{
				result=recallOrderServices.Create(model.Data.ToVDto());
			}
			catch (ActionStatusMessageException ex)
			{
				return new JsonResult(ex.Status);
			}
			return new JsonResult(new APIResponseIdViewModel(result.Id, ActionStatusMessage.Success));
		}
		[HttpGet]
		[AllowAnonymous]
		public IActionResult RecallOrder(Guid id)
		{
			var recall = _context.RecallOrders.Where(r => r.Id == id).FirstOrDefault();
			if (recall == null) return new JsonResult(ActionStatusMessage.Apply.Recall.NotExist);
			return new JsonResult(new RecallViewModel()
			{
				Data = recall.ToVDto()
			});
		}
	}
}
