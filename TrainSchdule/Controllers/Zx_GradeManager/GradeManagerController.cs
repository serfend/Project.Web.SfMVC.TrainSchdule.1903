using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.ZX;
using DAL.Entities;
using DAL.Entities.UserInfo;
using DAL.Entities.ZX.Phy;
using DAL.QueryModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Newtonsoft.Json;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;
using TrainSchdule.ViewModels.ZX;

namespace TrainSchdule.Controllers.Zx_GradeManager
{
	/// <summary>
	/// 岞訓用体能成绩
	/// </summary>
	[Route("ZX/[controller]")]
	public class GradeManagerController : ControllerBase
	{
		private readonly IPhyGradeServices phyGradeServices;
		private readonly IUsersService usersService;
		private readonly IGoogleAuthService googleAuthService;
		private readonly IUserActionServices userActionServices;
		private readonly ICurrentUserService currentUserService;

		/// <summary>
		///
		/// </summary>
		/// <param name="phyGradeServices"></param>
		/// <param name="usersService"></param>
		/// <param name="googleAuthService"></param>
		/// <param name="userActionServices"></param>
		/// <param name="currentUserService"></param>
		public GradeManagerController(IPhyGradeServices phyGradeServices, IUsersService usersService, IGoogleAuthService googleAuthService, IUserActionServices userActionServices, ICurrentUserService currentUserService)
		{
			this.phyGradeServices = phyGradeServices;
			this.usersService = usersService;
			this.googleAuthService = googleAuthService;
			this.userActionServices = userActionServices;
			this.currentUserService = currentUserService;
		}

		private void CheckPermission(GoogleAuthDataModel auth)
		{
			var authUser = auth.AuthUser(googleAuthService, usersService, currentUserService.CurrentUser?.Id);
			if (authUser == null) throw new ActionStatusMessageException(ActionStatusMessage.UserMessage.NotExist);
			if (!userActionServices.Permission(authUser.Application.Permission, DictionaryAllPermission.Grade.Subject, Operation.Update, authUser.Id, "")) throw new ActionStatusMessageException(auth.PermitDenied());
		}

		/// <summary>
		/// 编辑一个科目
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("Subject")]
		public IActionResult EditSubject([FromBody] PhySubjectDataModel model)
		{
			CheckPermission(model?.Auth);
			phyGradeServices.ModifySubject(model.Subject);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 编辑多个科目
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPut]
		[Route("Subjects")]
		public IActionResult EditSubjects([FromBody] PhySubjectsDataModel model)
		{
			CheckPermission(model?.Auth);
			foreach (var s in model.Subjects)
				phyGradeServices.ModifySubject(s);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 查询科目
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("Subject")]
		public IActionResult GetSubject(string name)
		{
			var r = phyGradeServices.FindSubject(name);
			if (r == null) return new JsonResult(ActionStatusMessage.Grade.Subject.NotExist);
			return new JsonResult(new EntityViewModel<GradePhySubject>(r));
		}

		/// <summary>
		/// 获取单个成绩结果及标准
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[Route("SingleResult")]
		[HttpPost]
		public IActionResult GetSingleResult([FromBody] PhySingleGradeDataModel model)
		{
			var baseUser = GetUser(model);
			var result = GetResult(model, baseUser);
			return new JsonResult(new PhySingleGradeViewModel()
			{
				Data = result
			});
		}

		/// <summary>
		/// 获取多组成绩查询
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[Route("MutilResult")]
		[HttpPost]
		public IActionResult GetMutilResult([FromBody] PhyGradeDataModel model)
		{
			var list = new List<PhySingleGradeDataModel>(model.Queries);
			var resultList = new List<PhySingleGradeDataModel>();

			for (int i = 0; i < list.Count; i++)
			{
				var m = list[i];
				var baseUser = GetUser(m);
				resultList.Add(GetResult(m, baseUser));
			}
			return new JsonResult(new PhyGradesViewModel()
			{
				Data = new PhyGradeDataModel() { Queries = resultList }
			});
		}

		private UserBaseInfo GetUser(PhySingleGradeDataModel model)
		{
			if (model?.User == null) throw new ActionStatusMessageException(ActionStatusMessage.UserMessage.NoId);

			var baseUser = model.User.UserName == null ? model.User.User : usersService.Get(model.User.UserName)?.BaseInfo;
			if (baseUser == null) throw new ActionStatusMessageException((ActionStatusMessage.UserMessage.NotExist));
			return baseUser;
		}

		/// <summary>
		/// 获取符合条件的科目
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("Subjects")]
		public IActionResult GetSubjects([FromBody] PhySingleGradeDataModel model)
		{
			var baseUser = GetUser(model);
			var result = new List<IEnumerable<GradePhySubject>>();
			foreach (var subject in model.Subjects)
			{
				var r = GetSubjects(subject, baseUser);
				result.Add(r);
			}
			return new JsonResult(new EntitiesListViewModel<IEnumerable<GradePhySubject>>(result));
		}

		private IEnumerable<GradePhySubject> GetSubjects(PhyGradeQueryDataModel subject, UserBaseInfo baseUser)
			=> phyGradeServices.GetSubjectsByName(new QueryUserGradeViewModel()
			{
				Names = new QueryByString()
				{
					Arrays = subject.Subject?.Split("|")
				},
				Groups = new QueryByString()
				{
					Value = subject.Group
				}
			}, baseUser);

		/// <summary>
		/// 获取单个成绩结果及标准
		/// 科目名称以|分割
		/// </summary>
		/// <param name="model"></param>
		/// <param name="baseUser"></param>
		/// <returns></returns>
		private PhySingleGradeDataModel GetResult(PhySingleGradeDataModel model, UserBaseInfo baseUser)
		{
			foreach (var subject in model.Subjects)
			{
				var subjectItem = GetSubjects(subject, baseUser).FirstOrDefault();
				if (subjectItem == null)
				{
					subject.Grade = -1;
					subject.Standard = "无效科目";
					continue;
				}
				subject.Group = subjectItem.Group;
				subject.Name = subjectItem.Alias;
				var standard = phyGradeServices.GetStandard(subjectItem, baseUser);
				subject.Standard = standard.ToRawValue();
				if (model.NeedCaculateGrade)
					subject.Grade = phyGradeServices.GetGrade(standard, subject.RawValue);
			}
			return model;
		}
	}
}