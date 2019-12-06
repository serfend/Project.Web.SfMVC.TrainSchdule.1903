using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels;
using TrainSchdule.ViewModels.Apply;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

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
		private readonly IRecallOrderServices recallOrderServices;
		private readonly IUserActionServices _userActionServices;
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
		/// <param name="recallOrderServices"></param>
		public ApplyController(IUsersService usersService, ICurrentUserService currentUserService, IApplyService applyService, ICompaniesService companiesService, IVerifyService verifyService, ApplicationDbContext context, IGoogleAuthService authService, IVocationCheckServices vocationCheckServices, IRecallOrderServices recallOrderServices, IUserActionServices userActionServices)
		{
			_usersService = usersService;
			_currentUserService = currentUserService;
			_applyService = applyService;
			_companiesService = companiesService;
			_verifyService = verifyService;
			_context = context;
			_authService = authService;
			_vocationCheckServices = vocationCheckServices;
			this.recallOrderServices = recallOrderServices;
			_userActionServices = userActionServices;
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
					List = BLL.Extensions.ApplyExtensions.ApplyStaticExtensions.StatusDic
				}
			}) ;
		}
		/// <summary>
		/// 提交申请的基础信息
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(typeof(APIResponseIdViewModel),0)]
		public async Task<IActionResult> BaseInfo([FromBody]SubmitBaseInfoViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			var targetUser = _usersService.Get(model.Id);
			if(targetUser==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			var userModel = new SubmitBaseInfoViewModel()
			{//重写前端传回的数据
				Id = model.Id,
				Company = targetUser.CompanyInfo.Company.Code,
				Duties= targetUser.CompanyInfo.Duties.Name,
				Phone = model.Phone??targetUser.SocialInfo.Phone,
				RealName=targetUser.BaseInfo.RealName,
				Settle=targetUser.SocialInfo.Settle,
				VocationTargetAddress=model.VocationTargetAddress,
				VocationTargetAddressDetail=model.VocationTargetAddressDetail,
			};
			var m = userModel.ToVDTO( _usersService);
			m.CreateBy = _currentUserService.CurrentUser;
			var info = await _applyService.SubmitBaseInfoAsync(m);
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

			var m = model.ToVDTO(_context, _vocationCheckServices,model.VocationType=="正休");
			if (m.VocationPlace == null) return new JsonResult(ActionStatusMessage.Static.AdminDivision.NoSuchArea);
			var targetUser = _usersService.Get(model.Id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var vocationInfo = _usersService.VocationInfo(targetUser);
			switch (model.VocationType)
			{
				case "正休":
					if (model.OnTripLength > 0 && vocationInfo.MaxTripTimes <= vocationInfo.OnTripTimes) return new JsonResult(ActionStatusMessage.Apply.Request.TripTimesExceed);
					if (model.VocationLength > vocationInfo.LeftLength) return new JsonResult(new Status(ActionStatusMessage.Apply.Request.NoEnoughVocation.status, $"已无足够假期可以使用，超出{model.VocationLength - vocationInfo.LeftLength}天"));
					if (model.VocationLength < 5) return new JsonResult(ActionStatusMessage.Apply.Request.VocationLengthTooShort);
					if (model.OnTripLength < 0) return new JsonResult(ActionStatusMessage.Apply.Request.Default);
					break;
				case "事假":
					m.VocationAdditionals = null;
					m.OnTripLength = 0;
					break;
				case "病休":
					m.VocationAdditionals = null;
					m.OnTripLength = 0;
					break;
				default:
					return new JsonResult(ActionStatusMessage.Apply.Request.InvalidVocationType);
			}


			var info = _applyService.SubmitRequest(m);
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
			var r = model.Verify.Verify(_verifyService);
			if (r != "") return new JsonResult(new Status(ActionStatusMessage.Account.Auth.Verify.Invalid.status, r));
			var dto = model.ToVDTO();
			var apply = _applyService.Submit(dto);
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
			if (!model.Auth.Verify(_authService))return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var authByUser = _usersService.Get(model.Auth.AuthByUserID);
			if (authByUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			Guid.TryParse(model.Id, out var id);
			var apply = _applyService.Get(id);
			if (apply == null) return new JsonResult(ActionStatusMessage.Apply.NotExist);
			if(!_userActionServices.Permission(apply.BaseInfo.From.Application.Permission, DictionaryAllPermission.Apply.Default, Operation.Update, authByUser.Id, apply.BaseInfo.From.CompanyInfo.Company.Code))return new JsonResult(ActionStatusMessage.Account.Auth.Permission.Default);

			var ua=_userActionServices.Log(UserOperation.RemoveApply, apply.BaseInfo.From.Id,$"通过{authByUser.Id}移除{apply.Create}创建的{apply.RequestInfo.VocationLength}天休假申请");
			if (!(apply.Status == AuditStatus.NotPublish || apply.Status == AuditStatus.NotSave || apply.Status == AuditStatus.Withdrew)) return new JsonResult(ActionStatusMessage.Apply.Operation.StatusInvalid.CanNotDelete);
			
			_applyService.Delete(apply);
			_userActionServices.Status(ua, true);
			return new JsonResult(ActionStatusMessage.Success);
		}
		
		#endregion
	}
}
