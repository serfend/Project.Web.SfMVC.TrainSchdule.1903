using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.Extensions.Common;
using TrainSchdule.ViewModels;
using TrainSchdule.ViewModels.Common;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers.Static
{
	/// <summary>
	/// 通用设置管理
	/// </summary>
	[Route("[controller]")]
	public partial class CommonController : Controller
	{
		private readonly ApplicationDbContext context;
		private readonly IUsersService usersService;
		private readonly ICurrentUserService currentUserService;
		private readonly IGoogleAuthService authService;

		/// <summary>
		///
		/// </summary>
		/// <param name="context"></param>
		/// <param name="usersService"></param>
		/// <param name="currentUserService"></param>
		/// <param name="authService"></param>
		public CommonController(ApplicationDbContext context, IUsersService usersService, ICurrentUserService currentUserService, IGoogleAuthService authService)
		{
			this.context = context;
			this.usersService = usersService;
			this.currentUserService = currentUserService;
			this.authService = authService;
		}

		/// <summary>
		/// 获取更新记录
		/// </summary>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("UpdateVersion")]
		public IActionResult GetUpdateVersion(int pageIndex = 0, int pageSize = 20)
		{
			var list = context.ApplicationUpdateRecordsDb.OrderByDescending(r => r.Create).SplitPage(pageIndex, pageSize).Result;
			return new JsonResult(new ApplicationUpdateRecordViewModel()
			{
				Data = new ApplicationUpdateRecordDataModel()
				{
					List = list.Item1,
					TotalCount = list.Item2
				}
			});
		}

		/// <summary>
		/// 创建或修改
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[Route("UpdateVersion")]
		[HttpPost]
		public IActionResult AddUpdateVersion([FromBody]ApplicationUpdateRecordUpdateViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(ModelState.ToModel());
			var authBy = model.Auth.AuthUser(authService, currentUserService.CurrentUser?.Id);
			if (authBy != "root") return new JsonResult(model.Auth.PermitDenied());
			var mList = model.Data.List;
			foreach (var m in mList)
			{
				var r = context.ApplicationUpdateRecordsDb.Where(rec => rec.Version == m.Version).FirstOrDefault();
				bool isAdd = false;
				if (r == null)
				{
					if (m.IsRemoved) return new JsonResult(r.NotExist());
					r = new ApplicationUpdateRecord();
					isAdd = true;
				}
				r = m.ToModel(r);
				if (m.IsRemoved) r.Remove();
				if (isAdd)
					context.ApplicationUpdateRecords.Add(r);
				else
					context.ApplicationUpdateRecords.Update(r);
			}
			context.SaveChanges();
			return new JsonResult(ActionStatusMessage.Success);
		}
	}
}