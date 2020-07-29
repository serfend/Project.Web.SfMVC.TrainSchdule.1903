using BLL.Extensions.ApplyExtensions;
using BLL.Extensions.ApplyExtensions.ApplyAuditStreamExtension;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.DTO.Apply;
using DAL.DTO.Apply.ApplyAuditStreamDTO;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainSchdule.ViewModels;
using TrainSchdule.ViewModels.Apply.ApplyAuditStream;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;
using static DAL.DTO.Apply.ApplyAuditStreamDTO.ApplyAuditStreamSolutionRuleDto;

namespace TrainSchdule.Controllers.Apply.AuditStream
{
	/// <summary>
	/// 申请流程规则管理，仅管理员可设置
	/// </summary>
	public partial class ApplyAuditStreamController : Controller
	{
		private readonly IApplyAuditStreamServices applyAuditStreamServices;
		private readonly ApplicationDbContext context;
		private readonly IGoogleAuthService googleAuthService;
		private readonly IUsersService usersService;
		private readonly ICurrentUserService currentUserService;
		private readonly IUserActionServices userActionServices;
		private readonly ICompaniesService companiesService;

		/// <summary>
		///
		/// </summary>
		/// <param name="applyAuditStreamServices"></param>
		/// <param name="context"></param>
		/// <param name="googleAuthService"></param>
		/// <param name="usersService"></param>
		/// <param name="currentUserService"></param>
		/// <param name="userActionServices"></param>
		/// <param name="companiesService"></param>
		public ApplyAuditStreamController(IApplyAuditStreamServices applyAuditStreamServices, ApplicationDbContext context, IGoogleAuthService googleAuthService, IUsersService usersService, ICurrentUserService currentUserService, IUserActionServices userActionServices, ICompaniesService companiesService)
		{
			this.applyAuditStreamServices = applyAuditStreamServices;
			this.context = context;
			this.googleAuthService = googleAuthService;
			this.usersService = usersService;
			this.currentUserService = currentUserService;
			this.userActionServices = userActionServices;
			this.companiesService = companiesService;
		}

		private ApiResult CheckPermission(User u, MembersFilterDto filter, string companyRegion, string prevRegion)
		{
			var regionResult = CheckRegion(u, companyRegion, prevRegion);
			if (filter == null || regionResult.Status != 0) return regionResult;
			if (u == null) return (ActionStatusMessage.UserMessage.NotExist);
			var targetCompanies = filter.Companies;
			foreach (var targetCompany in targetCompanies)
			{
				var permit = userActionServices.Permission(u?.Application?.Permission, DictionaryAllPermission.Apply.AuditStream, Operation.Create, u.Id, targetCompany);
				var targetCompanyItem = companiesService.GetById(targetCompany);
				if (!targetCompany.StartsWith(companyRegion)) return (new ApiResult(ActionStatusMessage.Account.Auth.Invalid.Default.Status, $"包含的单位{targetCompanyItem?.Name}({targetCompany})的越权"));
				if (!permit) return (new ApiResult(ActionStatusMessage.Account.Auth.Invalid.Default.Status, $"不具有{targetCompanyItem?.Name}({targetCompany})的权限"));
			}
			return ActionStatusMessage.Success;
		}

		private ApiResult CheckRegion(User u, string companyRegion, string prevRegion)
		{
			if ((companyRegion == "" || prevRegion == "") && u.Id.ToLower() != "root")
				return new ApiResult(ActionStatusMessage.Account.Auth.Invalid.Default.Status, "通用作用域需要管理员权限");
			if (companyRegion == null || prevRegion == null) return ActionStatusMessage.ApplyMessage.AuditStreamMessage.InvalidRegion;
			var newP = userActionServices.Permission(u?.Application?.Permission, DictionaryAllPermission.Apply.AuditStream, Operation.Create, u.Id, companyRegion);
			if (!newP)
			{
				var targetCompanyItem = companiesService.GetById(companyRegion);
				return (new ApiResult(ActionStatusMessage.Account.Auth.Invalid.Default.Status, $"不具有新作用域{targetCompanyItem?.Name}({companyRegion})的权限"));
			}
			if (!prevRegion.StartsWith(companyRegion))
			{
				var prevP = userActionServices.Permission(u?.Application?.Permission, DictionaryAllPermission.Apply.AuditStream, Operation.Create, u.Id, prevRegion);
				if (!prevP)
				{
					var targetCompanyItem = companiesService.GetById(companyRegion);
					return (new ApiResult(ActionStatusMessage.Account.Auth.Invalid.Default.Status, $"不具有原作用域{targetCompanyItem?.Name}({companyRegion})的权限"));
				}
			}
			return new ApiResult();
		}

		private ApiResult CheckPermissionNodes(DAL.Entities.UserInfo.User u, IEnumerable<ApplyAuditStreamNodeAction> nodes)
		{
			var result = ActionStatusMessage.Success;
			// 获取第一个低权限的节点，如果不存在则获取任何一个节点
			foreach (var node in nodes)
			{
				result = CheckPermission(u, node.ToDtoModel(), node.RegionOnCompany, node.RegionOnCompany);
				if (result.Status != 0) return result;
			}
			return result;
		}
	}
}