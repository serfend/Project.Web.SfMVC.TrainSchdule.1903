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
		[HttpGet]
		public IActionResult FromUser(string id)
		{
			id = id ?? _currentUserService.CurrentUser?.Id;
			var targetUser = _usersService.Get(id);
			if(targetUser==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			var list = _applyService.Find(a => a.BaseInfo.From.Id == id&&!a.Hidden);
			return new JsonResult(new ApplyListViewModel()
			{
				Data = new ApplyListDataModel()
				{
					List = list.Select(a=>a.ToDTO())
				}
			});
		}

		[HttpGet]
		public IActionResult FromCompany(string id)
		{
			id = id ?? _currentUserService.CurrentUser?.CompanyInfo?.Company?.Code;
			var targetCompany = _companiesService.Get(id);
			if (targetCompany == null) return new JsonResult(ActionStatusMessage.Company.NotExist);
			var list = _applyService.Find(a => a.BaseInfo.From.CompanyInfo.Company.Code == id && !a.Hidden);
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
