using BLL.Extensions.ApplyExtensions.ApplyAuditStreamExtension;
using BLL.Helpers;
using DAL.DTO.Apply.ApplyAuditStreamDTO;
using DAL.Entities.ApplyInfo;
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

namespace TrainSchdule.Controllers.Apply.AuditStream
{
	public partial class ApplyAuditStreamController
	{
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
					auditUser = usersService.GetById(model.Auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}

			var checkExist = applyAuditStreamServices.EditSolution(model.Name);
			if (checkExist != null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStreamMessage.StreamSolution.AlreadyExist);

			var list = new List<ApplyAuditStreamNodeAction>();
			var errorList = new StringBuilder();
			foreach (var node in model.Nodes)
			{
				applyAuditStreamServices.EditNode(node,model.EntityType, (n) =>
				{
					if (n == null) ModelState.AddModelError(node, "节点不存在");
					else list.Add(n);
					return false;
				});
			}
			//  检查新增实体权限
			var result = CheckPermission(auditUser, null, model.CompanyRegion, model.CompanyRegion);
			if (result.Status != 0) return new JsonResult(result);

			// 检查每个节点权限
			result = CheckPermissionNodes(auditUser, list);
			if (result.Status != 0) return new JsonResult(result);
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));

			var r = applyAuditStreamServices.NewSolution(list, model.Name, model.CompanyRegion, model.Description);
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
					auditUser = usersService.GetById(model.Auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}

			var node = applyAuditStreamServices.GetSolution(model.Id);
			if (node == null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStreamMessage.StreamSolution.NotExist);
			var result = CheckPermission(auditUser, null, model.CompanyRegion, node.RegionOnCompany);
			if (result.Status != 0) return new JsonResult(result);

			var list = new List<ApplyAuditStreamNodeAction>();
			var errorList = new StringBuilder();
			foreach (var no in model.Nodes)
			{
				applyAuditStreamServices.EditNode(no,model.EntityType, (n) =>
				{
					if (n == null) ModelState.AddModelError("节点列表", $"节点{no}不存在");
					else list.Add(n);
					return false;
				});
			}
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			//  检查每个节点权限
			result = CheckPermissionNodes(auditUser, list);
			if (result.Status != 0) return new JsonResult(result);

			node.Description = model.Description;
			node.Nodes = string.Join("##", list.Select(i => i.Name).ToArray());
			node.Name = model.Name;
			node.RegionOnCompany = model.CompanyRegion;
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
			if (checkExist == null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStreamMessage.StreamSolution.NotExist);
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
					auditUser = usersService.GetById(auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}

			ApplyAuditStream node = applyAuditStreamServices.EditSolution(name);
			if (node == null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStreamMessage.StreamSolution.NotExist);
			// 检查删除目标的权限
			var result = CheckPermission(auditUser, null, node.RegionOnCompany, node.RegionOnCompany);
			if (result != null && result.Status != 0) return new JsonResult(result);

			var nStr = (node.Nodes?.Length ?? 0) == 0 ? Array.Empty<string>() : node.Nodes.Split("##");
			var nList = context.ApplyAuditStreamNodeActionDb.Where(node => nStr.Contains(node.Name));
			// 检查包含节点的权限
			result = CheckPermissionNodes(auditUser, nList);
			if (result != null && result.Status != 0) return new JsonResult(result);
			node.Remove();
			context.ApplyAuditStreams.Update(node);
			context.SaveChanges();

			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 查询所有符合条件的解决方案
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("ApplyAuditStream/StreamSolutionQuery")]
		public IActionResult StreamSolutionQuery(string companyRegion, int pageIndex = 0, int pageSize = 100)
		{
			var result = context.ApplyAuditStreamsDb
					.Where(n => companyRegion.Contains(n.RegionOnCompany))
					.OrderByDescending(a => a.Create)
					.Skip(pageSize * pageIndex)
					.Take(pageSize)
					.Select(s => s.ToDtoModel().ToVDtoModel(applyAuditStreamServices, usersService, companiesService))
					.AsEnumerable();
			return new JsonResult(new EntitiesListViewModel<ApplyAuditStreamVDto>(result));
		}

		#endregion Solution
	}
}