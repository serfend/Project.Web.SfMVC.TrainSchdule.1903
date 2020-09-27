using BLL.Extensions.ApplyExtensions;
using BLL.Extensions.ApplyExtensions.ApplyAuditStreamExtension;
using BLL.Helpers;
using DAL.Entities.ApplyInfo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Apply.ApplyAuditStream;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;
using static DAL.DTO.Apply.ApplyAuditStreamDTO.ApplyAuditStreamSolutionRuleDto;

namespace TrainSchdule.Controllers.Apply.AuditStream
{
	public partial class ApplyAuditStreamController
	{
		#region Rule

		/// <summary>
		/// 创建一个审批流解决方案规则
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("ApplyAuditStream/StreamSolutionRule")]
		public IActionResult AddStreamSolutionRule([FromBody] StreamSolutionRuleCreateDataModel model)
		{
			var auditUser = currentUserService.CurrentUser;
			if (model.Auth?.AuthByUserID != null && auditUser?.Id != model.Auth?.AuthByUserID)
			{
				if (model.Auth.Verify(googleAuthService, currentUserService.CurrentUser?.Id))
					auditUser = usersService.GetById(model.Auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}

			ApplyAuditStreamSolutionRule checkExist = applyAuditStreamServices.EditSolutionRule(model.Name);
			if (checkExist != null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStreamMessage.StreamSolutionRule.AlreadyExist);

			// 判断新增实体的权限
			var result = CheckPermission(auditUser, model.Filter, model.CompanyRegion, model.CompanyRegion);
			if (result.Status != 0) return new JsonResult(result);

			ApplyAuditStream solution = applyAuditStreamServices.EditSolution(model.SolutionName);
			if (solution == null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStreamMessage.StreamSolution.NotExist);
			// 判断所需要调用的方案的权限
			result = CheckPermission(auditUser, null, solution.RegionOnCompany, solution.RegionOnCompany);
			if (result.Status != 0) return new JsonResult(result);

			var r = applyAuditStreamServices.NewSolutionRule(solution, model.Filter.ToModel<BaseMembersFilter>(), model.Name, model.CompanyRegion, model.Description, model.Priority, model.Enable);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 创建一个审批流解决方案规则
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPut]
		[Route("ApplyAuditStream/StreamSolutionRule")]
		public IActionResult EditStreamSolutionRule([FromBody] StreamSolutionRuleCreateDataModel model)
		{
			var auditUser = currentUserService.CurrentUser;
			if (model.Auth?.AuthByUserID != null && auditUser?.Id != model.Auth?.AuthByUserID)
			{
				if (model.Auth.Verify(googleAuthService, currentUserService.CurrentUser?.Id))
					auditUser = usersService.GetById(model.Auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}

			var n = applyAuditStreamServices.GetRule(model.Id);
			if (n == null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStreamMessage.StreamSolutionRule.NotExist);
			// 判断新增实体的权限
			var result = CheckPermission(auditUser, n.ToDtoModel(), model.CompanyRegion, n.RegionOnCompany);
			if (result.Status != 0) return new JsonResult(result);

			ApplyAuditStream solution = applyAuditStreamServices.EditSolution(model.SolutionName);
			if (solution == null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStreamMessage.StreamSolution.NotExist);
			// 判断所需方案的权限
			result = CheckPermission(auditUser, null, solution.RegionOnCompany, solution.RegionOnCompany);
			if (result.Status != 0) return new JsonResult(result);

			model.Filter.ToModel<ApplyAuditStreamSolutionRule>().ToApplyAuditStreamSolutionRule(n);
			n.Description = model.Description;
			n.Create = n.Create;
			n.Priority = model.Priority;
			n.Solution = solution;
			n.Enable = model.Enable;
			n.Name = model.Name;
			n.RegionOnCompany = model.CompanyRegion;
			context.ApplyAuditStreamSolutionRules.Update(n);
			context.SaveChanges();
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 查询一个审批流解决方案规则
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("ApplyAuditStream/StreamSolutionRule")]
		public IActionResult GetStreamSolutionRule(string name)
		{
			ApplyAuditStreamSolutionRule checkExist = null;
			applyAuditStreamServices.EditSolutionRule(name, (n) => { checkExist = n; return false; });
			if (checkExist == null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStreamMessage.StreamSolutionRule.NotExist);
			return new JsonResult(new StreamSolutionRuleViewModel()
			{
				Data = new StreamSolutionRuleDataModel()
				{
					Rule = checkExist.ToSolutionRuleDtoModel().ToSolutionRuleVDtoModel(usersService, companiesService)
				}
			});
		}

		/// <summary>
		/// 删除一个审批流解决方案规则
		/// </summary>
		/// <param name="name"></param>
		/// <param name="authByUserId"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("ApplyAuditStream/StreamSolutionRule")]
		public IActionResult DeleteStreamSolutionRule(string name, string authByUserId, string code)
		{
			var auth = new GoogleAuthDataModel()
			{
				AuthByUserID = authByUserId,
				Code = code
			};
			var auditUser = currentUserService.CurrentUser;
			if (auth?.AuthByUserID != null && auditUser?.Id != auth?.AuthByUserID)
			{
				if (auth.Verify(googleAuthService, currentUserService.CurrentUser?.Id))
					auditUser = usersService.GetById(auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}
			ApplyAuditStreamSolutionRule node = applyAuditStreamServices.EditSolutionRule(name);
			if (node == null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStreamMessage.StreamSolutionRule.NotExist);
			var result = CheckPermission(auditUser, node.ToDtoModel(), node.RegionOnCompany, node.RegionOnCompany);
			node.Remove();
			context.ApplyAuditStreamSolutionRules.Update(node);
			if (result != null && result.Status != 0) return new JsonResult(result);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 查询所有符合条件的规则
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("ApplyAuditStream/StreamSolutionRuleQuery")]
		public IActionResult StreamSolutionRuleQuery(string companyRegion, int pageIndex = 0, int pageSize = 100)
		{
			var result = context.ApplyAuditStreamSolutionRules
					.Where(n => companyRegion.Contains(n.RegionOnCompany)) // 取本级及上级内容
					.OrderByDescending(r => r.Priority)
					.OrderByDescending(r => r.Create)
					.Skip(pageIndex * pageSize)
					.Take(pageSize)
					.ToList().Select(r => r.ToSolutionRuleDtoModel().ToSolutionRuleVDtoModel(usersService, companiesService))
					.AsEnumerable();
			return new JsonResult(new EntitiesListViewModel<ApplyAuditStreamSolutionRuleVDto>(result));
		}

		#endregion Rule
	}
}