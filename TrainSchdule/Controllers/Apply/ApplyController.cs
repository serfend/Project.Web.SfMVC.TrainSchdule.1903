﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities.ApplyInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.ViewModels;
using TrainSchdule.ViewModels.Apply;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.Controllers.Apply
{
	/// <summary>
	/// 申请管理
	/// </summary>
	[Authorize]
	[Route("[controller]/[action]")]
	public partial class ApplyController: Controller
	{
		#region filed
		private readonly IUsersService _usersService;
		private readonly ICurrentUserService _currentUserService;
		private readonly IApplyService _applyService;
		private readonly IVocationCheckServices _vocationCheckServices;
		private readonly ApplicationDbContext _context;
		private readonly ICompaniesService _companiesService;
		private readonly IVerifyService _verifyService;
		private readonly IGoogleAuthService _authService;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="usersService"></param>
		/// <param name="currentUserService"></param>
		/// <param name="applyService"></param>
		/// <param name="companiesService"></param>
		/// <param name="verifyService"></param>
		/// <param name="context"></param>
		/// <param name="authService"></param>
		/// <param name="vocationCheckServices"></param>
		public ApplyController(IUsersService usersService, ICurrentUserService currentUserService, IApplyService applyService, ICompaniesService companiesService, IVerifyService verifyService, ApplicationDbContext context, IGoogleAuthService authService, IVocationCheckServices vocationCheckServices)
		{
			_usersService = usersService;
			_currentUserService = currentUserService;
			_applyService = applyService;
			_companiesService = companiesService;
			_verifyService = verifyService;
			_context = context;
			_authService = authService;
			_vocationCheckServices = vocationCheckServices;
		}

		#endregion

		#region Logic

		/// <summary>
		/// 获取申请所有可能的状态信息
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(Dictionary<int, AuditStatusMessage>),0)]
		public IActionResult AllStatus()
		{
			return new JsonResult(new ApplyAuditStatusViewModel()
			{
				Data = new ApplyAuditStatusDataModel()
				{
					List = ApplyExtensions.StatusDic
				}
			});
		}
		/// <summary>
		/// 提交申请的基础信息
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(APIResponseIdViewModel),0)]
		public async Task<IActionResult> BaseInfo([FromBody]SubmitBaseInfoViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			var targetUser = _usersService.Get(model.Id);
			if(targetUser==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			var info=await _applyService.SubmitBaseInfoAsync(Extensions.ApplyExtensions.ToVDTO(model,_usersService));
			if(info.Company==null)ModelState.AddModelError("company",$"不存在编号为{model.Company}的单位");
			if(info.Duties==null)ModelState.AddModelError("duties",$"不存在职务代码:{model.Duties}");
			if(info.Social.Address==null)ModelState.AddModelError("home",$"不存在的行政区划{model.HomeAddress}");
			if (!ModelState.IsValid)return new JsonResult(new ApiResponseModelStateErrorViewModel(info.Id, ModelState));
			return new JsonResult(new APIResponseIdViewModel(info.Id, ActionStatusMessage.Success));
		}

		/// <summary>
		/// 提交本次申请的请求信息
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(APIResponseIdViewModel),0)]
		public  IActionResult RequestInfo([FromBody] SubmitRequestInfoViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			var targetUser = _usersService.Get(model.Id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var info =  _applyService.SubmitRequest(Extensions.ApplyExtensions.ToVDTO(model,_context, _vocationCheckServices));
			if(info.VocationPlace==null) ModelState.AddModelError("home", $"不存在的行政区划{model.VocationPlace}");
			if (!ModelState.IsValid) return new JsonResult(new ApiResponseModelStateErrorViewModel(info.Id,ModelState));
			return new JsonResult(new APIResponseIdViewModel(info.Id,ActionStatusMessage.Success));
		}

		/// <summary>
		/// 创建申请
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(APIResponseIdViewModel),0)]

		public IActionResult Submit([FromBody] SubmitApplyViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			if(!model.Verify.Verify(_verifyService)) return new JsonResult(ActionStatusMessage.Account.Auth.Verify.Invalid);
			var apply = _applyService.Submit(Extensions.ApplyExtensions.ToVDTO(model));
			if(apply==null)return new JsonResult(ActionStatusMessage.Apply.Operation.Submit.Crash);
			if (apply.RequestInfo == null) return new JsonResult(ActionStatusMessage.Apply.Operation.Submit.NoRequestInfo);
			if(apply.BaseInfo==null)return new JsonResult(ActionStatusMessage.Apply.Operation.Submit.NoBaseInfo);
			if (apply.BaseInfo?.Company==null)return new JsonResult(ActionStatusMessage.Company.NotExist);
			if(apply.Response==null||!apply.Response.Any())return new JsonResult(ActionStatusMessage.Company.NoneCompanyBelong);
			 
			return new JsonResult(new APIResponseIdViewModel(apply.Id,ActionStatusMessage.Success));
		}
		/// <summary>
		/// 删除指定申请
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpDelete]
		[AllowAnonymous]
		[ProducesResponseType(typeof(APIResponseIdViewModel),0)]

		public IActionResult Submit([FromBody]ApplyRemoveViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			if (model.Auth==null||!_authService.Verify(model.Auth.Code,model.Auth.AuthByUserID))return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			Guid.TryParse(model.Id, out var id);
			var apply = _applyService.Get(id);
			if(apply==null)return new JsonResult(ActionStatusMessage.Apply.NotExist);
			_applyService.Delete(apply);
			return new JsonResult(ActionStatusMessage.Success);
		}
		
		#endregion
	}
}