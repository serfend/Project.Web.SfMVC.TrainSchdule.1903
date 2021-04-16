using Abp.Extensions;
using BLL.Extensions.ApplyExtensions;
using BLL.Extensions.ApplyExtensions.ApplyAuditStreamExtension;
using BLL.Helpers;
using DAL.DTO.Apply.ApplyAuditStreamDTO;
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

namespace TrainSchdule.Controllers.Apply.AuditStream
{
	public partial class ApplyAuditStreamController
	{
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
			if (model.Auth?.AuthByUserID.IsNullOrEmpty() == false && auditUser?.Id != model.Auth?.AuthByUserID)
			{
				if (model.Auth.Verify(googleAuthService, currentUserService.CurrentUser?.Id))
					auditUser = usersService.GetById(model.Auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}

			var prevNode = applyAuditStreamServices.EditNode(model.Name,model.Filter.EntityType);
			if (prevNode != null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStreamMessage.Node.AlreadyExist);

			var permit = CheckPermission(auditUser, model?.Filter, model.CompanyRegion, model.CompanyRegion);
			if (permit.Status != 0) return new JsonResult(permit);

			var r = applyAuditStreamServices.NewNode(model.Filter.ToModel<BaseMembersFilter>(), model.Name, model.CompanyRegion, model.Description, model.Filter.EntityType);
			if (DateTime.Now.Subtract(r.Create).TotalSeconds > 10) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStreamMessage.Node.AlreadyExist);
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
			if (model.Auth?.AuthByUserID.IsNullOrEmpty() == false && auditUser?.Id != model.Auth?.AuthByUserID)
			{
				if (model.Auth.Verify(googleAuthService, currentUserService.CurrentUser?.Id))
					auditUser = usersService.GetById(model.Auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}
			// 所有的编辑都需要使用id获取，否则将不支持修改名称
			var n = applyAuditStreamServices.GetNode(model.Id);
			if (n == null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStreamMessage.Node.NotExist);
			// 判断新增实体的权限
			var permit = CheckPermission(auditUser, model?.Filter, model.CompanyRegion, n.RegionOnCompany);
			if (permit.Status != 0) return new JsonResult(permit);
			// 通过ToModel将前端Filter注入到原库内
			model.Filter.ToModel<ApplyAuditStreamNodeAction>().ToApplyAuditStreamNodeAction(n);
			n.Description = model.Description;
			n.Create = n.Create;
			n.Name = model.Name;
			n.RegionOnCompany = model.CompanyRegion;
			context.ApplyAuditStreamNodeActions.Update(n);
			context.SaveChanges();
			return new JsonResult(ActionStatusMessage.Success);
		}

        /// <summary>
        /// 查询审批节点
        /// </summary>
        /// <param name="name"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        [HttpGet]
		[Route("ApplyAuditStream/StreamNode")]
		public IActionResult GetStreamNode(string name,string entityType)
		{
			var r = applyAuditStreamServices.EditNode(name,entityType);
			if (r == null) return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStreamMessage.Node.NotExist);
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
        /// <param name="entityType"></param>
        /// <param name="authByUserId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpDelete]
		[Route("ApplyAuditStream/StreamNode")]
		public IActionResult DeleteStreamNode(string name,string entityType, string authByUserId, string code)
		{
			var auth = new GoogleAuthDataModel()
			{
				AuthByUserID = authByUserId,
				Code = code
			};
			var auditUser = currentUserService.CurrentUser;
			if (auth?.AuthByUserID.IsNullOrEmpty() == false && auth?.AuthByUserID.IsNullOrEmpty() == false && auditUser?.Id != auth?.AuthByUserID)
			{
				if (auth.Verify(googleAuthService, currentUserService.CurrentUser?.Id))
					auditUser = usersService.GetById(auth.AuthByUserID);
				else return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			}
			var n = applyAuditStreamServices.EditNode(name,entityType);
			if (n != null)
			{
				// 判断此目标的权限
				var result = CheckPermission(auditUser, n.ToDtoModel(), n.RegionOnCompany, n.RegionOnCompany);
				if (result.Status == 0)
				{
					n.Remove();
					context.ApplyAuditStreamNodeActions.Update(n);
					context.SaveChanges();
				}
			}
			else
				return new JsonResult(ActionStatusMessage.ApplyMessage.AuditStreamMessage.Node.NotExist);
			return new JsonResult(ActionStatusMessage.Success);
		}

		// TODO 日后可以设置新增规则筛选器

		/// <summary>
		/// 查询所有符合条件的节点
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("ApplyAuditStream/StreamNodeQuery")]
		public IActionResult StreamNodeQuery(string companyRegion,string entityType, int pageIndex = 0, int pageSize = 100)
		{
			var list = context.ApplyAuditStreamNodeActionDb;
			var result = list
					.Where(n=>n.EntityType == entityType)
					.Where(n => companyRegion.Contains(n.RegionOnCompany))
					.OrderByDescending(a => a.Create)
					.Skip(pageIndex * pageSize)
					.Take(pageSize)
					.Select(n => n.ToNodeDtoModel(null).ToNodeVDtoModel(usersService, companiesService, null))
					.AsEnumerable();
			return new JsonResult(new EntitiesListViewModel<ApplyAuditStreamNodeActionVDto>(result));
		}

		#endregion Node
	}
}