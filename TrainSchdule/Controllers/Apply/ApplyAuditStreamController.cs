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
		private readonly IUserActionServices userActionServices;

		public ApplyAuditStreamController(IApplyAuditStreamServices applyAuditStreamServices, ApplicationDbContext context, IGoogleAuthService googleAuthService, IUsersService usersService, IUserActionServices userActionServices)
		{
			this.applyAuditStreamServices = applyAuditStreamServices;
			this.context = context;
			this.googleAuthService = googleAuthService;
			this.usersService = usersService;
			this.userActionServices = userActionServices;
		}

		private ApiResult CheckPermission(GoogleAuthDataModel auth, MembersFilterDto filter)
		{
			if (!auth.Verify(googleAuthService, null)) return (ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var u = usersService.Get(auth?.AuthByUserID);
			if (u == null) return (ActionStatusMessage.User.NotExist);
			// 如果要新增相对，则需要管理员权限
			if (filter?.CompanyRefer != null || filter?.CompanyCodeLength?.Count() > 0 || filter?.CompanyTags?.Count() > 0)
				if (!userActionServices.Permission(u?.Application?.Permission, DictionaryAllPermission.Apply.AuditStream, Operation.Create, u.Id, "root"))
					return (ActionStatusMessage.Account.Auth.Invalid.Default);
			var targetCompaines = filter?.Companies;
			foreach (var targetCompany in targetCompaines)
			{
				var permit = userActionServices.Permission(u?.Application?.Permission, DictionaryAllPermission.Apply.AuditStream, Operation.Create, u.Id, targetCompany);
				if (!permit) return (new ApiResult(ActionStatusMessage.Account.Auth.Invalid.Default.Status, $"不具有{targetCompany}的权限"));
			}
			return ActionStatusMessage.Success;
		}

		private ApiResult CheckPermissionNodes(GoogleAuthDataModel auth, IEnumerable<ApplyAuditStreamNodeAction> nodes)
		{
			var result = ActionStatusMessage.Success;
			// 获取第一个低权限的节点，如果不存在则获取任何一个节点
			var validNode = nodes.Where(no => no.CompanyRefer == null).FirstOrDefault();
			if (validNode == null) validNode = nodes.FirstOrDefault();
			if (validNode != null)
				result = CheckPermission(auth, ((MembersFilter)validNode).ToDtoModel());
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
		public IActionResult AddStreamNode([FromBody]StreamNodeCreateDataModel model)
		{
			ApplyAuditStreamNodeAction checkExist = null;
			applyAuditStreamServices.EditNode(model.Name, (n) => { checkExist = n; return false; });
			if (checkExist != null) return new JsonResult(ActionStatusMessage.Apply.AuditStream.Node.AlreadyExist);

			var permit = CheckPermission(model?.Auth, model?.Filter);
			if (permit.Status != 0) return new JsonResult(permit);

			var r = applyAuditStreamServices.NewNode(model.Filter.ToModel(), model.Name, model.Description);
			if (DateTime.Now.Subtract(r.Create).TotalSeconds > 10) return new JsonResult(ActionStatusMessage.Apply.AuditStream.Node.AlreadyExist);
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
			ApplyAuditStreamNodeAction r = null;
			applyAuditStreamServices.EditNode(name, (n) => { r = n; return false; });
			if (r == null) return new JsonResult(ActionStatusMessage.Apply.AuditStream.Node.NotExist);
			return new JsonResult(new StreamNodeViewModel()
			{
				Data = new StreamNodeDataModel()
				{
					Node = r.ToNodeDtoModel()
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
			ApiResult result = null;
			try
			{
				applyAuditStreamServices.EditNode(name, (n) =>
				{
					if (n != null)
					{
						result = CheckPermission(auth, ((MembersFilter)n).ToDtoModel());
						if (result.Status != 0) return false;
						context.ApplyAuditStreamNodeActions.Remove(n);
						context.SaveChanges();
					}
					else
						result = ActionStatusMessage.Apply.AuditStream.Node.NotExist;
					return false;
				});
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
		public IActionResult AddStreamSolution([FromBody]StreamSolutionCreateDataModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
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

			result = CheckPermissionNodes(model?.Auth, list);
			if (result.Status != 0) return new JsonResult(result);
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));

			ApplyAuditStream checkExist = null;
			applyAuditStreamServices.EditSolution(model.Name, (n) =>
			{
				checkExist = n;
				return false;
			});
			if (checkExist != null) return new JsonResult(ActionStatusMessage.Apply.AuditStream.StreamSolution.AlreadyExist);

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
		public IActionResult EditStreamSolution([FromBody]StreamSolutionCreateDataModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
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

			result = CheckPermissionNodes(model?.Auth, list);
			if (result.Status != 0) return new JsonResult(result);
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));

			ApplyAuditStream checkExist = null;
			applyAuditStreamServices.EditSolution(model.Name, (n) =>
			{
				checkExist = n;
				if (n != null)
				{
					n.Description = model.Description;
					n.Nodes = string.Join("##", list.Select(i => i.Name).ToArray());
					return true;
				}
				return false;
			});
			if (checkExist == null) return new JsonResult(ActionStatusMessage.Apply.AuditStream.StreamSolution.NotExist);

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
			applyAuditStreamServices.EditSolution(name, (n) => { checkExist = n; return false; });
			if (checkExist == null) return new JsonResult(ActionStatusMessage.Apply.AuditStream.StreamSolution.NotExist);
			return new JsonResult(new StreamSolutionViewModel()
			{
				Data = new StreamSolutionDataModel()
				{
					Solution = checkExist.ToDtoModel()
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
			ApiResult result = null;

			ApplyAuditStream checkExist = null;
			applyAuditStreamServices.EditSolution(name, (n) =>
			{
				checkExist = n;
				if (n != null)
				{
					var nStr = n.Nodes?.Length == 0 ? Array.Empty<string>() : n.Nodes.Split("##");
					var nList = context.ApplyAuditStreamNodeActions.Where(node => nStr.Contains(node.Name));
					result = CheckPermissionNodes(auth, nList);
					if (result.Status != 0) return false;

					context.ApplyAuditStreams.Remove(n);
					context.SaveChanges();
				}
				else
					result = ActionStatusMessage.Apply.AuditStream.StreamSolution.NotExist;
				return false;
			});
			if (checkExist == null) return new JsonResult(ActionStatusMessage.Apply.AuditStream.StreamSolution.NotExist);
			if (result.Status != 0) return new JsonResult(result);
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
		public IActionResult AddStreamSolutionRule([FromBody]StreamSolutionRuleCreateDataModel model)
		{
			ApiResult result = null;
			ApplyAuditStreamSolutionRule checkExist = null;
			applyAuditStreamServices.EditSolutionRule(model.Name, (n) =>
			{
				checkExist = n;
				if (n != null)
				{
					result = CheckPermission(model?.Auth, n.ToDtoModel());
					return false;
				}
				else
					result = ActionStatusMessage.Success;

				return false;
			});
			if (checkExist != null) return new JsonResult(ActionStatusMessage.Apply.AuditStream.StreamSolutionRule.AlreadyExist);
			if (result.Status != 0) return new JsonResult(result);

			ApplyAuditStream solution = null;
			applyAuditStreamServices.EditSolution(model.SolutionName, (n) => { solution = n; return false; });
			if (solution == null) return new JsonResult(ActionStatusMessage.Apply.AuditStream.StreamSolution.NotExist);

			var r = applyAuditStreamServices.NewSolutionRule(solution, model.Filter.ToModel(), model.Name, model.Description, model.Priority, model.Enable);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 查询一个审批流解决方案规则
		/// </summary>
		/// <param name="name"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("ApplyAuditStream/StreamSolutionRule")]
		public IActionResult GetStreamSolutionRule(string name)
		{
			ApplyAuditStreamSolutionRule checkExist = null;
			applyAuditStreamServices.EditSolutionRule(name, (n) => { checkExist = n; return false; });
			if (checkExist == null) return new JsonResult(ActionStatusMessage.Apply.AuditStream.StreamSolutionRule.NotExist);
			return new JsonResult(new StreamSolutionRuleViewModel()
			{
				Data = new StreamSolutionRuleDataModel()
				{
					Rule = checkExist.ToSolutionRuleDtoModel()
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
			ApiResult result = null;

			ApplyAuditStreamSolutionRule checkExist = null;
			applyAuditStreamServices.EditSolutionRule(name, (n) =>
			{
				checkExist = n;
				if (n != null)
				{
					result = CheckPermission(auth, n.ToDtoModel());
					if (result.Status != 0) return false;
					context.ApplyAuditStreamSolutionRules.Remove(n);
					context.SaveChanges();
				};
				return false;
			});
			if (checkExist == null) return new JsonResult(ActionStatusMessage.Apply.AuditStream.StreamSolutionRule.NotExist);
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
					List = context.ApplyAuditStreamNodeActions.Select(n => n.ToNodeDtoModel())
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
					List = context.ApplyAuditStreams.Select(s => s.ToDtoModel())
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
					List = context.ApplyAuditStreamSolutionRules.OrderByDescending(r => r.Priority).ToList().Select(r => r.ToSolutionRuleDtoModel())
				}
			});
		}

		#endregion List Query
	}
}