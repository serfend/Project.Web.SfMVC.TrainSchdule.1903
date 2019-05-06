using System.Linq;
using System.Threading.Tasks;
using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.ViewModels.Apply;
using TrainSchdule.ViewModels.System;
using TrainSchdule.Web.ViewModels;

namespace TrainSchdule.Web.Controllers
{
	[Authorize]
	[Route("[controller]/[action]")]
	public class ApplyController: Controller
	{
		#region filed
		private readonly IUsersService _usersService;
		private readonly ICurrentUserService _currentUserService;
		private readonly IApplyService _applyService;
		private readonly ApplicationDbContext _context;
		private readonly ICompaniesService _companiesService;
		private readonly IVerifyService _verifyService;


		public ApplyController(IUsersService usersService, ICurrentUserService currentUserService, IApplyService applyService, ICompaniesService companiesService, IVerifyService verifyService, ApplicationDbContext context)
		{
			_usersService = usersService;
			_currentUserService = currentUserService;
			_applyService = applyService;
			_companiesService = companiesService;
			_verifyService = verifyService;
			_context = context;
		}

		#endregion

		#region Logic

		[HttpGet]
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
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> BaseInfo([FromBody]SubmitBaseInfoViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			var targetUser = _usersService.Get(model.Id);
			if(targetUser==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			var info=await _applyService.SubmitBaseInfoAsync(Extensions.ApplyExtensions.ToVDTO(model,_usersService));
			if(info.Company==null)ModelState.AddModelError("company",$"不存在编号为{model.Company}的单位");
			if(info.Duties==null)ModelState.AddModelError("duties",$"不存在职务代码:{model.Duties}");
			if(info.Social.Address==null)ModelState.AddModelError("home",$"不存在的行政区划{model.HomeAddress}");
			if (!ModelState.IsValid)return new JsonResult(new APIResponseModelStateErrorViewModel(info.Id, ModelState));
			return new JsonResult(new APIResponseIdViewModel(info.Id, ActionStatusMessage.Success));
		}

		[HttpPost]
		[AllowAnonymous]
		public  IActionResult RequestInfo([FromBody] SubmitRequestInfoViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			var targetUser = _usersService.Get(model.Id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var info =  _applyService.SubmitRequest(Extensions.ApplyExtensions.ToVDTO(model,_context));
			if(info.VocationPlace==null) ModelState.AddModelError("home", $"不存在的行政区划{model.VocationPlace}");
			if (!ModelState.IsValid) return new JsonResult(new APIResponseModelStateErrorViewModel(info.Id,ModelState));
			return new JsonResult(new APIResponseIdViewModel(info.Id,ActionStatusMessage.Success));
		}

		[HttpPost]
		[AllowAnonymous]
		public IActionResult Submit([FromBody] SubmitApplyViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			if(!model.Verify.Verify(_verifyService)) return new JsonResult(ActionStatusMessage.Account.Auth.Verify.Invalid);
			var apply = _applyService.Submit(Extensions.ApplyExtensions.ToVDTO(model));
			if(apply.BaseInfo?.Company==null)return new JsonResult(ActionStatusMessage.Company.NotExist);
			if(apply.Response==null||!apply.Response.Any())return new JsonResult(ActionStatusMessage.Company.NoneCompanyBelong);
			return new JsonResult(new APIResponseIdViewModel(apply.Id,ActionStatusMessage.Success));
		}
		#endregion
	}
}
