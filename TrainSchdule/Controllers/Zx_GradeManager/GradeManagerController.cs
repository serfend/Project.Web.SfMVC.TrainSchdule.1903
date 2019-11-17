using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.ZX;
using DAL.Entities;
using DAL.Entities.ZX.Phy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TrainSchdule.ViewModels.Verify;
using TrainSchdule.ViewModels.ZX;

namespace TrainSchdule.Controllers.Zx_GradeManager
{
	/// <summary>
	/// 岞訓用体能成绩
	/// </summary>
	[Route("ZX/[controller]/[action]")]
	[Authorize]
	public class GradeManagerController : ControllerBase
	{
		private readonly IPhyGradeServices phyGradeServices;
		private readonly IUsersService usersService;
		private readonly IGoogleAuthService googleAuthService;
		public GradeManagerController(IPhyGradeServices phyGradeServices, IUsersService usersService, IGoogleAuthService googleAuthService)
		{
			this.phyGradeServices = phyGradeServices;
			this.usersService = usersService;
			this.googleAuthService = googleAuthService;
		}
		/// <summary>
		/// 添加一个科目
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult Subject([FromBody]PhySubjectDataModel model)
		{
			if (!model.Verify(googleAuthService)) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var actionUser = usersService.Get(model.AuthByUserID);
			if (actionUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			if (!actionUser.Application.Permission.Check(DictionaryAllPermission.Grade.Subject, Operation.Update, new Company())) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			phyGradeServices.AddSubject(model.Subject);
			return new JsonResult(ActionStatusMessage.Success);
		}
		/// <summary>
		/// 查询科目
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Subject(string name)
		{
			var r = phyGradeServices.FindSubject(name);
			if (r == null) return new JsonResult(ActionStatusMessage.Grade.Subject.NotExist);
			return new JsonResult(new PhySubjectViewModel()
			{
				Data = r
			});
		}
		/// <summary>
		/// 获取单个成绩结果及标准
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost]
		public IActionResult SingleResult([FromBody]PhySingleGradeDataModel model)
		{
			 var result = GetResult(ref model);
			return new JsonResult(new PhySingleGradeViewModel(result)
			{
				Data = model
			});
		}
		[AllowAnonymous]
		[HttpPost]
		public IActionResult MutilResult([FromBody]PhyGradeDataModel model)
		{
			var list = new List<PhySingleGradeDataModel>(model.Queries);
			var resultList = new List<PhySingleGradeDataModel>();
			var finalResult = new Status(0,"");
			for(int i =0;i<list.Count;i++)
			{
				var m = list[i];
				var result = GetResult(ref m);
				if (result.status != 0) finalResult = result;
				resultList.Add(m);
			}
			return new JsonResult(new PhyGradesViewModel()
			{
				Data = new PhyGradeDataModel() { Queries= resultList }
			});
		}
		/// <summary>
		/// 获取单个成绩结果及标准
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		private Status GetResult(ref PhySingleGradeDataModel model)
		{
			if (model?.User == null) return ActionStatusMessage.User.NoId;

			var baseUser = model.User.UserName == null ? model.User.User : usersService.Get(model.User.UserName)?.BaseInfo;
			if (baseUser == null) return (ActionStatusMessage.User.NotExist);
			foreach (var subject in model.Subjects)
			{
				var subjectItem = phyGradeServices.GetSubjectByName(subject.Subject,baseUser);
				if (subjectItem == null)
				{
					subject.Grade = -1;
					subject.Standard = "无效科目";
					continue;
				}
				var standard = phyGradeServices.GetStandard(subjectItem, baseUser);
				subject.Standard = standard.ToRawValue();
				subject.Grade = phyGradeServices.GetGrade(standard, subject.RawValue);
			}
			return ActionStatusMessage.Success;
		}
	}
}