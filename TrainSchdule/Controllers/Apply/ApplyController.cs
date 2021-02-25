using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.Common;
using BLL.Services.ApplyServices;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.Common.DataDictionary;
using DAL.Entities.UserInfo;
using DAL.Entities.Vacations;
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

		private readonly IUsersService usersService;
		private readonly ICurrentUserService currentUserService;
		private readonly IApplyService applyService;
		private readonly IVacationCheckServices vacationCheckServices;
		private readonly ApplicationDbContext context;
		private readonly ICompaniesService companiesService;
		private readonly IVerifyService verifyService;
		private readonly IGoogleAuthService authService;
		private readonly IRecallOrderServices recallOrderServices;
		private readonly IUserActionServices userActionServices;
        private readonly IDataDictionariesServices dataDictionariesServices;

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
        /// <param name="dataDictionariesServices"></param>
        public ApplyController(IUsersService usersService, ICurrentUserService currentUserService, IApplyService applyService, IVacationCheckServices vacationCheckServices, ApplicationDbContext context, ICompaniesService companiesService, IVerifyService verifyService, IGoogleAuthService authService, IRecallOrderServices recallOrderServices, IUserActionServices userActionServices, IDataDictionariesServices dataDictionariesServices)
		{
			this.usersService = usersService;
			this.currentUserService = currentUserService;
			this.applyService = applyService;
			this.vacationCheckServices = vacationCheckServices;
			this.context = context;
			this.companiesService = companiesService;
			this.verifyService = verifyService;
			this.authService = authService;
			this.recallOrderServices = recallOrderServices;
			this.userActionServices = userActionServices;
            this.dataDictionariesServices = dataDictionariesServices;
        }

		#endregion filed

	}
	public partial class ApplyController
	{
		#region Logic

		/// <summary>
		/// 获取申请所有可能的状态信息
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(Dictionary<string, AuditStatusMessage>), 0)]
		public IActionResult AllStatus()
		{
			var vacationTypes = context.VacationTypes.Where(t => !t.IsRemoved)
				.ToList()
				.Select(t => new KeyValuePair<string, VacationType>(t.Name, t));
			var applyStatus = dataDictionariesServices.GetByGroupName( "ApplyStatus").ToList();
			var dictList = applyStatus.Select(s => new KeyValuePair<string, AuditStatusMessage>(s.Value.ToString(), new AuditStatusMessage(s.Value, s.Key, s.Alias, s.Color)
			{
				Acessable = s.Description.Split("##", StringSplitOptions.RemoveEmptyEntries)
			}));

			var actions = dataDictionariesServices.GetByGroupName("ApplyAction")
				.ToList()
				.Select(s => new KeyValuePair<string, ActionByUserItem>(s.Key, new ActionByUserItem(s.Key, s.Alias, s.Color, s.Description)));

			var executeStatus = dataDictionariesServices.GetByGroupName("ApplyExecuteStatus")
				.ToList()
				.Select(d => new KeyValuePair<string, CommonDataDictionary>(d.Value.ToString(), d));
			return new JsonResult(new ApplyAuditStatusViewModel()
			{
				Data = new ApplyAuditStatusDataModel()
				{
					List = new Dictionary<string, AuditStatusMessage>(dictList),
					Actions = new Dictionary<string, ActionByUserItem>(actions),
					VacationTypes = new Dictionary<string, VacationType>(vacationTypes),
					ExecuteStatus = new Dictionary<string, CommonDataDictionary>(executeStatus)
				}
			}); ;
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
			var targetUser = usersService.GetById(model.Id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
			var userModel = new SubmitBaseInfoViewModel()
			{//重写前端传回的数据
				Id = model.Id,
				Company = targetUser.CompanyInfo.Company?.Code,
				Duties = targetUser.CompanyInfo.Duties?.Name,
				Phone = model.Phone ?? targetUser.SocialInfo.Phone,
				RealName = targetUser.BaseInfo.RealName,
				Settle = targetUser.SocialInfo.Settle,
				VacationTargetAddress = model.VacationTargetAddress,
				VacationTargetAddressDetail = model.VacationTargetAddressDetail,
			};
			var m = userModel.ToVDTO(usersService);
			m.CreateBy = currentUserService.CurrentUser;
			var info = await applyService.SubmitBaseInfoAsync(m);
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
			var targetUser = usersService.GetById(model.Id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
			var m = model.ToVDTO(context);
			if (m.VacationPlace == null) return new JsonResult(ActionStatusMessage.StaticMessage.AdminDivision.NoSuchArea);
			try
			{
				var info = await applyService.SubmitRequestAsync(targetUser, m).ConfigureAwait(true);
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
			model.Verify.Verify(verifyService);
			var dto = model.ToVDTO();
			var apply = applyService.Submit(dto);
			if (apply == null) return new JsonResult(ActionStatusMessage.ApplyMessage.Default);
			if (apply.RequestInfo == null) return new JsonResult(ActionStatusMessage.ApplyMessage.Operation.Submit.NoRequestInfo);
			if (apply.BaseInfo == null) return new JsonResult(ActionStatusMessage.ApplyMessage.Operation.Submit.NoBaseInfo);
			if (apply.BaseInfo?.Company == null) return new JsonResult(ActionStatusMessage.CompanyMessage.NotExist);
			userActionServices.Log(UserOperation.CreateApply, apply.BaseInfo.From.Id, null, true, ActionRank.Warning);
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
			if (!model.Auth.Verify(authService, currentUserService.CurrentUser?.Id)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var authByUser = usersService.GetById(model.Auth.AuthByUserID);
			if (authByUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
			Guid.TryParse(model.Id, out var id);
			var apply = applyService.GetById(id);
			if (apply == null) return new JsonResult(ActionStatusMessage.ApplyMessage.NotExist);
			var targetUser = apply.BaseInfo.From;
			// 本人及有权限者可操作
			if (
				authByUser.Id != targetUser.Id
				&& !userActionServices.Permission(authByUser.Application.Permission, DictionaryAllPermission.Apply.Default, Operation.Remove, authByUser.Id, targetUser.CompanyInfo.Company.Code)
			) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var ua = userActionServices.Log(UserOperation.RemoveApply, targetUser.Id, $"通过{authByUser.Id}移除{apply.Create}创建的{apply.RequestInfo.VacationLength}天休假申请", false, ActionRank.Danger);
			if (!(apply.Status == AuditStatus.NotPublish || apply.Status == AuditStatus.NotSave)) 
				return new JsonResult(ActionStatusMessage.ApplyMessage.Operation.StatusInvalid.CanNotDelete);
			await applyService.Delete(apply);
			userActionServices.Status(ua, true);
			return new JsonResult(ActionStatusMessage.Success);
		}

		#endregion Logic
	}
}