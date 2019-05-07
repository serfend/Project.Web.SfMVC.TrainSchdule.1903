using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Extensions;
using BLL.Helpers;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.ViewModels.Apply;

namespace TrainSchdule.Controllers
{
	public partial class ApplyController
	{
		/// <summary>
		/// 来自此用户发布的
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult FromUser(string id)
		{
			id = id ?? _currentUserService.CurrentUser?.Id;
			var targetUser = _usersService.Get(id);
			if(targetUser==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			var list = _applyService.GetApplyBySubmitUser(id);
			return new JsonResult(new ApplyListViewModel()
			{
				Data = new ApplyListDataModel()
				{
					List = list.Select(a=>a.ToDTO())
				}
			});
		}
		/// <summary>
		/// 交给此用户审批的（选定单位）
		/// </summary>
		/// <param name="id"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult ToUser(string id,string code)
		{
			id = id ?? _currentUserService.CurrentUser?.Id;
			var targetUser = _usersService.Get(id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var targetCompany = _companiesService.Get(code);
			if(targetCompany==null)return new JsonResult(ActionStatusMessage.Company.NotExist);
			if(!_companiesService.CheckManagers(code, id))return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default) ;
			return GetCompanyApply(code);
		}
		/// <summary>
		/// 来自此单位的
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult FromCompany(string code)
		{
			code = code ?? _currentUserService.CurrentUser?.CompanyInfo?.Company?.Code;
			var targetCompany = _companiesService.Get(code);
			if (targetCompany == null) return new JsonResult(ActionStatusMessage.Company.NotExist);
			return new JsonResult(new ApplyListViewModel()
			{
				Data = new ApplyListDataModel()
				{
					List = _applyService.GetApplyBySubmitCompany(code).Select(a=>a.ToDTO())
				}
			});
		}
		/// <summary>
		/// 交给此单位审批的
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult ToCompany(string code)
		{
			code = code ?? _currentUserService.CurrentUser?.CompanyInfo?.Company?.Code;
			var targetCompany = _companiesService.Get(code);
			if (targetCompany == null) return new JsonResult(ActionStatusMessage.Company.NotExist);
			return GetCompanyApply(targetCompany.Code);
		}
		private IActionResult GetCompanyApply(string code)
		{
			var list = _applyService.GetApplyByToAuditCompany(code);
			return new JsonResult(new ApplyListViewModel()
			{
				Data = new ApplyListDataModel()
				{
					List = list.Select(a => a.ToDTO())
				}
			});
		}
		
	}
}
