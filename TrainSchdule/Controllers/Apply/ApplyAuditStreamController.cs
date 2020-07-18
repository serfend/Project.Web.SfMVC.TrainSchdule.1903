using BLL.Extensions.ApplyExtensions;
using BLL.Extensions.ApplyExtensions.ApplyAuditStreamExtension;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.DTO.Apply;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainSchdule.ViewModels;
using TrainSchdule.ViewModels.Apply.ApplyAuditStream;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers.Apply
{
	/// <summary>
	/// 申请流程规则管理，仅管理员可设置
	/// </summary>
	public class ApplyAuditStreamController : Controller
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

		private ApiResult CheckPermission(DAL.Entities.UserInfo.User u, MembersFilterDto filter)
		{
			if (u == null) return (ActionStatusMessage.UserMessage.NotExist);
			// 如果要新增相对，则需要管理员权限
			if (filter?.CompanyRefer != null || filter?.CompanyCodeLength?.Count() > 0 || filter?.CompanyTags?.Count() > 0)
				if (!userActionServices.Permission(u?.Application?.Permission, DictionaryAllPermission.Apply.AuditStream, Operation.Create, u.Id, "root", "检查权限"))
					return (ActionStatusMessage.Account.Auth.Invalid.Default);
			var targetCompanies = filter?.Companies;
			if ((targetCompanies?.Count() ?? 0) == 0)
			{
				if (u.Id.ToLower() != "root") return new ApiResult(ActionStatusMessage.Account.Auth.Invalid.Default.Status, "通用规则需要管理员权限");
			}
			foreach (var targetCompany in targetCompanies)
			{
				var permit = userActionServices.Permission(u?.Application?.Permission, DictionaryAllPermission.Apply.AuditStream, Operation.Create, u.Id, targetCompany);
				var targetCompanyItem = companiesService.GetById(targetCompany);
				if (!permit) return (new ApiResult(ActionStatusMessage.Account.Auth.Invalid.Default.Status, $"不具有{targetCompanyItem?.Name}({targetCompany})的权限"));
			}
			return ActionStatusMessage.Success;
		}

		private ApiResult CheckPermissionNodes(DAL.Entities.UserInfo.User u, IEnumerable<ApplyAuditStreamNodeAction> nodes)
		{
			var result = ActionStatusMessage.Success;
			// 获取第一个低权限的节点，如果不存在则获取任何一个节点
			var validNode = nodes.Where(no => no.CompanyRefer == null).FirstOrDefault();
			if (validNode == null) validNode = nodes.FirstOrDefault();
			if (validNode != null)
				result = CheckPermission(u, ((IMembersFilter)validNode).ToDtoModel());
			return result;
		}

		#region Node

		/// <summary>
		/// 创建一个审批节点
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("ApplyAuditStream/StreamNode")]
		public IActionResult AddStreamNode([FromBody] StreamNodeCreateDataModel model)
		{
			var auditUser = currentUserService.CurrentUser;
			if (model.Auth?.AuthByUserID != null && auditUser?.Id != model.Auth?.AuthByUserID)
			{
				if (model.Auth.Verify(googleAuthService, currentUserService.CurrentUser?.Id))
					auditUser = usersService.Get(model.Auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}

			var checkExist = applyAuditStreamServices.EditNode(model.Name);
			if (checkExist != null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStream.Node.AlreadyExist);

			var permit = CheckPermission(auditUser, model?.Filter);
			if (permit.Status != 0) return new JsonResult(permit);

			var r = applyAuditStreamServices.NewNode(model.Filter.ToModel<BaseMembersFilter>(), model.Name, model.Description);
			if (DateTime.Now.Subtract(r.Create).TotalSeconds > 10) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStream.Node.AlreadyExist);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 修改Node
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPut]
		[Route("ApplyAuditStream/StreamNode")]
		public IActionResult EditStreamNode([FromBody] StreamNodeCreateDataModel model)
		{
			var auditUser = currentUserService.CurrentUser;
			if (model.Auth?.AuthByUserID != null && auditUser?.Id != model.Auth?.AuthByUserID)
			{
				if (model.Auth.Verify(googleAuthService, currentUserService.CurrentUser?.Id))
					auditUser = usersService.Get(model.Auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}
			var permit = CheckPermission(auditUser, model?.Filter);
			if (permit.Status != 0) return new JsonResult(permit);
			var n = applyAuditStreamServices.GetNode(model.Id);
			if (n == null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStream.Node.NotExist);
			model.Filter.ToModel<ApplyAuditStreamNodeAction>().ToApplyAuditStreamNodeAction(n);
			n.Description = model.Description;
			n.Create = n.Create;
			n.Name = model.Name;
			context.ApplyAuditStreamNodeActions.Update(n);
			context.SaveChanges();

			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 查询审批节点
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("ApplyAuditStream/StreamNode")]
		public IActionResult GetStreamNode(string name)
		{
			var r = applyAuditStreamServices.EditNode(name);
			if (r == null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStream.Node.NotExist);
			return new JsonResult(new StreamNodeViewModel()
			{
				Data = new StreamNodeDataModel()
				{
					Node = r.ToNodeDtoModel().ToNodeVDtoModel(usersService, companiesService)
				}
			});
		}

		/// <summary>
		/// 删除一个审批节点
		/// </summary>
		/// <param name="name"></param>
		/// <param name="authByUserId"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("ApplyAuditStream/StreamNode")]
		public IActionResult DeleteStreamNode(string name, string authByUserId, string code)
		{
			var auth = new GoogleAuthDataModel()
			{
				AuthByUserID = authByUserId,
				Code = code
			};
			var auditUser = currentUserService.CurrentUser;
			if (auth?.AuthByUserID != null && auth?.AuthByUserID != null && auditUser?.Id != auth?.AuthByUserID)
			{
				if (auth.Verify(googleAuthService, currentUserService.CurrentUser?.Id))
					auditUser = usersService.Get(auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}

			ApiResult result = null;
			try
			{
				var n = applyAuditStreamServices.EditNode(name);
				if (n != null)
				{
					result = CheckPermission(auditUser, ((IMembersFilter)n).ToDtoModel());
					if (result.Status == 0)
					{
						context.ApplyAuditStreamNodeActions.Remove(n);
						context.SaveChanges();
					}
				}
				else
					return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStream.Node.NotExist);
			}
			catch (Exception ex)
			{
				return new JsonResult(new ApiResult(-104, ex.Message));
			}
			return new JsonResult(result.Status == 0 ? ActionStatusMessage.Success : result);
		}

		#endregion Node

		#region Solution

		/// <summary>
		/// 创建一个审批流解决方案
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("ApplyAuditStream/StreamSolution")]
		public IActionResult AddStreamSolution([FromBody] StreamSolutionCreateDataModel model)
		{
			var auditUser = currentUserService.CurrentUser;
			if (model.Auth?.AuthByUserID != null && auditUser?.Id != model.Auth?.AuthByUserID)
			{
				if (model.Auth.Verify(googleAuthService, currentUserService.CurrentUser?.Id))
					auditUser = usersService.Get(model.Auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}

			ApiResult result = null;

			var list = new List<ApplyAuditStreamNodeAction>();
			var errorList = new StringBuilder();
			foreach (var node in model.Nodes)
			{
				applyAuditStreamServices.EditNode(node, (n) =>
				{
					if (n == null) ModelState.AddModelError(node, "节点不存在");
					else list.Add(n);
					return false;
				});
			}

			result = CheckPermissionNodes(auditUser, list);
			if (result.Status != 0) return new JsonResult(result);
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));

			ApplyAuditStream checkExist = null;
			checkExist = applyAuditStreamServices.EditSolution(model.Name);
			if (checkExist != null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStream.StreamSolution.AlreadyExist);

			var r = applyAuditStreamServices.NewSolution(list, model.Name, model.Description);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 编辑审批流方案
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPut]
		[Route("ApplyAuditStream/StreamSolution")]
		public IActionResult EditStreamSolution([FromBody] StreamSolutionCreateDataModel model)
		{
			var auditUser = currentUserService.CurrentUser;
			if (model.Auth?.AuthByUserID != null && auditUser?.Id != model.Auth?.AuthByUserID)
			{
				if (model.Auth.Verify(googleAuthService, currentUserService.CurrentUser?.Id))
					auditUser = usersService.Get(model.Auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}

			ApiResult result = null;

			var list = new List<ApplyAuditStreamNodeAction>();
			var errorList = new StringBuilder();
			foreach (var no in model.Nodes)
			{
				applyAuditStreamServices.EditNode(no, (n) =>
				{
					if (n == null) ModelState.AddModelError("节点列表", $"节点{no}不存在");
					else list.Add(n);
					return false;
				});
			}

			result = CheckPermissionNodes(auditUser, list);
			if (result.Status != 0) return new JsonResult(result);
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));

			var node = applyAuditStreamServices.GetSolution(model.Id);

			if (node == null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStream.StreamSolution.NotExist);
			node.Description = model.Description;
			node.Nodes = string.Join("##", list.Select(i => i.Name).ToArray());
			node.Name = model.Name;
			context.ApplyAuditStreams.Update(node);
			context.SaveChanges();
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 查询一个审批流解决方案
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("ApplyAuditStream/StreamSolution")]
		public IActionResult GetStreamSolution(string name)
		{
			ApplyAuditStream checkExist = null;
			checkExist = applyAuditStreamServices.EditSolution(name);
			if (checkExist == null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStream.StreamSolution.NotExist);
			return new JsonResult(new StreamSolutionViewModel()
			{
				Data = new StreamSolutionDataModel()
				{
					Solution = checkExist.ToDtoModel().ToVDtoModel(applyAuditStreamServices, usersService, companiesService)
				}
			});
		}

		/// <summary>
		/// 删除一个审批流解决方案
		/// </summary>
		/// <param name="name"></param>
		/// <param name="authByUserId"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("ApplyAuditStream/StreamSolution")]
		public IActionResult DeleteStreamSolution(string name, string authByUserId, string code)
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
					auditUser = usersService.Get(auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}

			ApiResult result = null;

			ApplyAuditStream checkExist = null;
			checkExist = applyAuditStreamServices.EditSolution(name);

			if (checkExist == null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStream.StreamSolution.NotExist);

			var nStr = (checkExist.Nodes?.Length ?? 0) == 0 ? Array.Empty<string>() : checkExist.Nodes.Split("##");
			var nList = context.ApplyAuditStreamNodeActions.Where(node => nStr.Contains(node.Name));
			result = CheckPermissionNodes(auditUser, nList);
			if (result != null && result.Status != 0) return new JsonResult(result);
			context.ApplyAuditStreams.Remove(checkExist);
			context.SaveChanges();

			return new JsonResult(ActionStatusMessage.Success);
		}

		#endregion Solution

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
					auditUser = usersService.Get(model.Auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}
			ApiResult result = null;
			ApplyAuditStreamSolutionRule checkExist = null;
			var permit = CheckPermission(auditUser, model.Filter);
			if (permit != null && permit.Status != 0) return new JsonResult(permit);
			applyAuditStreamServices.EditSolutionRule(model.Name, (n) =>
			{
				checkExist = n;
				if (n != null)
				{
					result = CheckPermission(auditUser, n.ToDtoModel());
					return false;
				}
				else
					result = ActionStatusMessage.Success;

				return false;
			});
			if (checkExist != null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStream.StreamSolutionRule.AlreadyExist);
			if (result.Status != 0) return new JsonResult(result);

			ApplyAuditStream solution = null;
			solution = applyAuditStreamServices.EditSolution(model.SolutionName);
			if (solution == null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStream.StreamSolution.NotExist);

			var r = applyAuditStreamServices.NewSolutionRule(solution, model.Filter.ToModel<BaseMembersFilter>(), model.Name, model.Description, model.Priority, model.Enable);
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
					auditUser = usersService.Get(model.Auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}

			ApiResult result = null;
			ApplyAuditStream solution = null;
			solution = applyAuditStreamServices.EditSolution(model.SolutionName);
			if (solution == null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStream.StreamSolution.NotExist);
			var n = applyAuditStreamServices.GetRule(model.Id);
			if (n == null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStream.StreamSolutionRule.NotExist);
			result = CheckPermission(auditUser, n.ToDtoModel());
			if (result.Status == 0)
			{
				model.Filter.ToModel<ApplyAuditStreamSolutionRule>().ToApplyAuditStreamSolutionRule(n);
				n.Description = model.Description;
				n.Create = n.Create;
				n.Priority = model.Priority;
				n.Solution = solution;
				n.Enable = model.Enable;
				n.Name = model.Name;
				context.ApplyAuditStreamSolutionRules.Update(n);
				context.SaveChanges();
			}
			if (result.Status != 0) return new JsonResult(result);

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
			if (checkExist == null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStream.StreamSolutionRule.NotExist);
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
					auditUser = usersService.Get(auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}
			ApiResult result = null;

			ApplyAuditStreamSolutionRule checkExist = null;
			applyAuditStreamServices.EditSolutionRule(name, (n) =>
			{
				checkExist = n;
				if (n != null)
				{
					result = CheckPermission(auditUser, n.ToDtoModel());
					if (result.Status != 0) return false;
					context.ApplyAuditStreamSolutionRules.Remove(n);
					context.SaveChanges();
				};
				return false;
			});
			if (result != null && result.Status != 0) return new JsonResult(result);
			if (checkExist == null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStream.StreamSolutionRule.NotExist);
			return new JsonResult(ActionStatusMessage.Success);
		}

		#endregion Rule

		#region List Query

		// TODO 日后可以设置新增规则筛选器

		/// <summary>
		/// 查询所有符合条件的节点
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("ApplyAuditStream/StreamNodeQuery")]
		public IActionResult StreamNodeQuery()
		{
			return new JsonResult(new StreamNodeListViewModel()
			{
				Data = new StreamNodeListDataModel()
				{
					List = context.ApplyAuditStreamNodeActions.OrderByDescending(a => a.Create).Select(n => n.ToNodeDtoModel(null).ToNodeVDtoModel(usersService, companiesService, null))
				}
			});
		}

		/// <summary>
		/// 查询所有符合条件的解决方案
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("ApplyAuditStream/StreamSolutionQuery")]
		public IActionResult StreamSolutionQuery()
		{
			return new JsonResult(new AuditStreamSolutionListViewModel()
			{
				Data = new AuditStreamSolutionListDataModel()
				{
					List = context.ApplyAuditStreams.OrderByDescending(a => a.Create).Select(s => s.ToDtoModel().ToVDtoModel(applyAuditStreamServices, usersService, companiesService))
				}
			});
		}

		/// <summary>
		/// 查询所有符合条件的规则
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("ApplyAuditStream/StreamSolutionRuleQuery")]
		public IActionResult StreamSolutionRuleQuery()
		{
			return new JsonResult(new AuditStreamSolutionListRuleViewModel()
			{
				Data = new AuditStreamSolutionListRuleDataModel()
				{
					List = context.ApplyAuditStreamSolutionRules.OrderByDescending(r => r.Priority).OrderByDescending(r => r.Create).ToList().Select(r => r.ToSolutionRuleDtoModel().ToSolutionRuleVDtoModel(usersService, companiesService))
				}
			});
		}

		#endregion List Query
	}
}