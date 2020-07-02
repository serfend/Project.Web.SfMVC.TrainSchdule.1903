using BLL.Helpers;
using DAL.Entities;
using DAL.Entities.ZX.Grade;
using DAL.QueryModel.ZX;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.Controllers.Zx_GradeManager
{
	public partial class GradeManagerController
	{
		/// <summary>
		/// 编辑一个考核记录
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("Grade/Exam")]
		public IActionResult EditGrade([FromBody] EntityWithAuthDataModel<GradeExam> model)
		{
			var prev = context.GradeExams.Where(e => e.Name == model.Model.Name).FirstOrDefault();
			CheckPermission(model?.Auth, DictionaryAllPermission.Grade.Exam, model.Model.GetOperation(prev), prev?.HoldBy?.Code ?? "");
			gradeServices.ModifyExam(model.Model);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 获取考核记录
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("Grade/Exam")]
		public IActionResult GetGrade([FromBody] QueryGradeExamViewModel model)
		{
			var result = gradeServices.GetExams(model);
			return new JsonResult(new EntitiesListViewModel<GradeExam>(result.Item1, result.Item2));
		}
	}
}