using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
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

namespace TrainSchdule.Controllers.Apply
{
	/// <summary>
	/// 申请流程规则管理，仅管理员可设置
	/// </summary>
	[Authorize]
	[Route("[controller]/[action]")]
	public class ApplyAuditStreamController : Controller
	{
		private readonly IApplyAuditStreamServices applyAuditStreamServices;
		private readonly ApplicationDbContext context;

		public ApplyAuditStreamController(IApplyAuditStreamServices applyAuditStreamServices, ApplicationDbContext context)
		{
			this.applyAuditStreamServices = applyAuditStreamServices;
			this.context = context;
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
			var r = applyAuditStreamServices.NewNode(model.Filter, model.Name, model.Description);
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
					Node = r
				}
			});
		}

		/// <summary>
		/// 删除一个审批节点
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("ApplyAuditStream/StreamNode")]
		public IActionResult DeleteStreamNode(string name)
		{
			bool success = false;
			try
			{
				applyAuditStreamServices.EditNode(name, (n) =>
				{
					if (n != null)
					{
						success = true;
						context.ApplyAuditStreamNodeActions.Remove(n);
						context.SaveChanges();
					}
					return false;
				});
			}
			catch (Exception ex)
			{
				return new JsonResult(new ApiResult(-104, ex.Message));
			}
			return new JsonResult(success ? ActionStatusMessage.Success : ActionStatusMessage.Apply.AuditStream.Node.NotExist);
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
			ApplyAuditStream checkExist = null;
			applyAuditStreamServices.EditSolution(model.Name, (n) => { checkExist = n; return false; });
			if (checkExist != null) return new JsonResult(ActionStatusMessage.Apply.AuditStream.StreamSolution.AlreadyExist);
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
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			var r = applyAuditStreamServices.NewSolution(list, model.Name, model.Description);
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
					Solution = checkExist
				}
			});
		}

		/// <summary>
		/// 删除一个审批流解决方案
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("ApplyAuditStream/StreamSolution")]
		public IActionResult DeleteStreamSolution(string name)
		{
			ApplyAuditStream checkExist = null;
			applyAuditStreamServices.EditSolution(name, (n) =>
			{
				checkExist = n; if (n != null)
				{
					context.ApplyAuditStreams.Remove(n);
					context.SaveChanges();
				}; return false;
			});
			if (checkExist == null) return new JsonResult(ActionStatusMessage.Apply.AuditStream.StreamSolution.NotExist);
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
			ApplyAuditStreamSolutionRule checkExist = null;
			applyAuditStreamServices.EditSolutionRule(model.Name, (n) => { checkExist = n; return false; });
			if (checkExist != null) return new JsonResult(ActionStatusMessage.Apply.AuditStream.StreamSolutionRule.AlreadyExist);
			ApplyAuditStream solution = null;
			applyAuditStreamServices.EditSolution(model.SolutionName, (n) => { solution = n; return false; });
			if (solution == null) return new JsonResult(ActionStatusMessage.Apply.AuditStream.StreamSolution.NotExist);
			var r = applyAuditStreamServices.NewSolutionRule(solution, model.Filter, model.Name, model.Description, model.Priority, model.Enable);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 查询一个审批流解决方案规则
		/// </summary>
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
					Rule = checkExist
				}
			});
		}

		/// <summary>
		/// 删除一个审批流解决方案规则
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("ApplyAuditStream/StreamSolutionRule")]
		public IActionResult DeleteStreamSolutionRule(string name)
		{
			ApplyAuditStreamSolutionRule checkExist = null;
			applyAuditStreamServices.EditSolutionRule(name, (n) =>
			{
				checkExist = n; if (n != null)
				{
					context.ApplyAuditStreamSolutionRules.Remove(n);
					context.SaveChanges();
				}; return false;
			});
			if (checkExist == null) return new JsonResult(ActionStatusMessage.Apply.AuditStream.StreamSolutionRule.NotExist);
			return new JsonResult(ActionStatusMessage.Success);
		}

		#endregion Rule
	}
}