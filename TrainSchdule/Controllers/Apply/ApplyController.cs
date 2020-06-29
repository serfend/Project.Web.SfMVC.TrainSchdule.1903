using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Services.ApplyServices;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
	public partial class ApplyController : Controller
	{
		#region filed

		private readonly IUsersService _usersService;
		private readonly ICurrentUserService _currentUserService;
		private readonly IApplyService _applyService;
		private readonly IVacationCheckServices _vacationCheckServices;
		private readonly ApplicationDbContext _context;
		private readonly ICompaniesService _companiesService;
		private readonly IVerifyService _verifyService;
		private readonly IGoogleAuthService _authService;
		private readonly IRecallOrderServices recallOrderServices;
		private readonly IUserActionServices _userActionServices;
		private readonly IHostingEnvironment _hostingEnvironment;

		/// <summary>
		///
		/// </summary>
		/// <param name="usersService"></param>
		/// <param name="currentUserService"></param>
		/// <param name="applyService"></param>
		/// <param name="vacationCheckServices"></param>
		/// <param name="context"></param>
		/// <param name="companiesService"></param>
		/// <param name="verifyService"></param>
		/// <param name="authService"></param>
		/// <param name="recallOrderServices"></param>
		/// <param name="userActionServices"></param>
		/// <param name="hostingEnvironment"></param>
		public ApplyController(IUsersService usersService, ICurrentUserService currentUserService, IApplyService applyService, IVacationCheckServices vacationCheckServices, ApplicationDbContext context, ICompaniesService companiesService, IVerifyService verifyService, IGoogleAuthService authService, IRecallOrderServices recallOrderServices, IUserActionServices userActionServices, IHostingEnvironment hostingEnvironment)
		{
			_usersService = usersService;
			_currentUserService = currentUserService;
			_applyService = applyService;
			_vacationCheckServices = vacationCheckServices;
			_context = context;
			_companiesService = companiesService;
			_verifyService = verifyService;
			_authService = authService;
			this.recallOrderServices = recallOrderServices;
			_userActionServices = userActionServices;
			_hostingEnvironment = hostingEnvironment;
		}

		#endregion filed

		#region Logic

		/// <summary>
		/// 获取申请所有可能的状态信息
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(Dictionary<int, AuditStatusMessage>), 0)]
		public IActionResult AllStatus()
		{
			return new JsonResult(new ApplyAuditStatusViewModel()
			{
				Data = new ApplyAuditStatusDataModel()
				{
					List = BLL.Extensions.ApplyExtensions.ApplyStaticExtensions.StatusDic,
					Actions = BLL.Extensions.ApplyExtensions.ApplyStaticExtensions.ActionDic
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
		[ProducesResponseType(typeof(APIResponseIdViewModel), 0)]
		public async Task<IActionResult> BaseInfo([FromBody] SubmitBaseInfoViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			var targetUser = _usersService.Get(model.Id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
			var userModel = new SubmitBaseInfoViewModel()
			{//重写前端传回的数据
				Id = model.Id,
				Company = targetUser.CompanyInfo.Company.Code,
				Duties = targetUser.CompanyInfo.Duties.Name,
				Phone = model.Phone ?? targetUser.SocialInfo.Phone,
				RealName = targetUser.BaseInfo.RealName,
				Settle = targetUser.SocialInfo.Settle,
				VacationTargetAddress = model.VacationTargetAddress,
				VacationTargetAddressDetail = model.VacationTargetAddressDetail,
			};
			var m = userModel.ToVDTO(_usersService);
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
		[ProducesResponseType(typeof(APIResponseIdViewModel), 0)]
		public async Task<IActionResult> RequestInfo([FromBody] SubmitRequestInfoViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			var targetUser = _usersService.Get(model.Id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
			var m = model.ToVDTO(_context);
			if (m.VacationPlace == null) return new JsonResult(ActionStatusMessage.Static.AdminDivision.NoSuchArea);
			try
			{
				var info = await _applyService.SubmitRequestAsync(targetUser, m).ConfigureAwait(true);
				return new JsonResult(new APIResponseIdViewModel(info.Id, ActionStatusMessage.Success));
			}
			catch (ActionStatusMessageException ex)
			{
				return new JsonResult(ex.Status);
			}
		}

		/// <summary>
		/// 创建申请
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(APIResponseIdViewModel), 0)]
		public IActionResult Submit([FromBody] SubmitApplyViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			model.Verify.Verify(_verifyService);
			var dto = model.ToVDTO();
			var apply = _applyService.Submit(dto);
			if (apply == null) return new JsonResult(ActionStatusMessage.ApplyMessage.Default);
			if (apply.RequestInfo == null) return new JsonResult(ActionStatusMessage.ApplyMessage.Operation.Submit.NoRequestInfo);
			if (apply.BaseInfo == null) return new JsonResult(ActionStatusMessage.ApplyMessage.Operation.Submit.NoBaseInfo);
			if (apply.BaseInfo?.Company == null) return new JsonResult(ActionStatusMessage.CompanyMessage.NotExist);

			return new JsonResult(new APIResponseIdViewModel(apply.Id, ActionStatusMessage.Success));
		}

		/// <summary>
		/// 删除指定申请
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpDelete]
		[AllowAnonymous]
		[ProducesResponseType(typeof(APIResponseIdViewModel), 0)]
		public async Task<IActionResult> Submit([FromBody] ApplyRemoveViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			if (!model.Auth.Verify(_authService, _currentUserService.CurrentUser?.Id)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var authByUser = _usersService.Get(model.Auth.AuthByUserID);
			if (authByUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
			Guid.TryParse(model.Id, out var id);
			var apply = _applyService.GetById(id);
			if (apply == null) return new JsonResult(ActionStatusMessage.ApplyMessage.NotExist);
			if (!_userActionServices.Permission(authByUser.Application.Permission, DictionaryAllPermission.Apply.Default, Operation.Remove, authByUser.Id, apply.BaseInfo.From.CompanyInfo.Company.Code)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var ua = _userActionServices.Log(UserOperation.RemoveApply, apply.BaseInfo.From.Id, $"通过{authByUser.Id}移除{apply.Create}创建的{apply.RequestInfo.VacationLength}天休假申请", false, ActionRank.Danger);
			if (!(apply.Status == AuditStatus.NotPublish || apply.Status == AuditStatus.NotSave)) return new JsonResult(ActionStatusMessage.ApplyMessage.Operation.StatusInvalid.CanNotDelete);
			await _applyService.Delete(apply);
			_userActionServices.Status(ua, true);
			return new JsonResult(ActionStatusMessage.Success);
		}

		#endregion Logic
	}
}