using System;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using BLL.Extensions.ApplyExtensions.ApplyAuditStreamExtension;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.ApplyInfo;
using BLL.Interfaces.Audit;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.ApplyInfo.DailyApply;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels;
using TrainSchdule.ViewModels.Apply;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.User;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers.Apply
{
	/// <summary>
	/// 审批流调用
	/// </summary>
	[Authorize]
	[Route("[controller]/[action]")]
	public class ApplyAuditController : Controller
	{
        private readonly IUsersService usersService;
        private readonly ICurrentUserService currentUserService;
        private readonly IUserActionServices userActionServices;
        private readonly IAuditStreamServices auditStreamServices;
        private readonly IGoogleAuthService googleAuthService;
        private readonly ApplicationDbContext context;
        private readonly IApplyVacationService applyService;

        /// <summary>
        /// 
        /// </summary>
        public ApplyAuditController(IUsersService usersService,ICurrentUserService currentUserService,IUserActionServices userActionServices, IAuditStreamServices auditStreamServices,IGoogleAuthService googleAuthService,ApplicationDbContext context, IApplyVacationService applyService)
        {
            this.usersService = usersService;
            this.currentUserService = currentUserService;
            this.userActionServices = userActionServices;
            this.auditStreamServices = auditStreamServices;
            this.googleAuthService = googleAuthService;
            this.context = context;
            this.applyService = applyService;
        }

        /// <summary>
        /// 此用户提交申请后，将生成的审批流
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        [AllowAnonymous]
		[Route("{entityType}")]
		[HttpGet]
		[ProducesResponseType(typeof(UserAuditStreamDataModel), 0)]
		public IActionResult AuditStream(string id,string entityType)
		{
			var targetUser = usersService.CurrentQueryUser(id);
			var a = new AuditStreamModel();
			auditStreamServices.InitAuditStream(ref a,entityType,targetUser);
			return new JsonResult(new UserAuditStreamViewModel()
			{
				Data = new UserAuditStreamDataModel()
				{
					Steps = a.ApplyAllAuditStep.Select(s => s.ToDtoModel()),
					SolutionName = a.ApplyAuditStreamSolutionRule.Solution.Name
				}
			});
		}

	}
}