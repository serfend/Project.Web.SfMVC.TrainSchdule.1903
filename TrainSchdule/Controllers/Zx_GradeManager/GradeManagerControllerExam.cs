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
using TrainSchdule.ViewModels.ZX;

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
		public IActionResult EditExam([FromBody] ExamModifyViewModel model)
		{
			var prev = context.GradeExams.Where(e => e.Name == model.Name).FirstOrDefault();
			var m = model.ToModel(context);
			var operation = m.GetOperation(prev);
			if (prev?.HoldBy != null && prev.HoldBy?.Code != m?.HoldBy?.Code) CheckPermission(model?.Auth, DictionaryAllPermission.Grade.Exam, operation, prev.HoldBy.Code, "移出原单位");
			CheckPermission(model?.Auth, DictionaryAllPermission.Grade.Exam, operation, m.HoldBy.Code);
			gradeServices.ModifyExam(m);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 获取考核记录
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("Grade/Exam")]
		public IActionResult GetExam([FromBody] QueryGradeExamViewModel model)
		{
			var result = gradeServices.GetExams(model);
			return new JsonResult(new EntitiesListViewModel<GradeExam>(result.Item1, result.Item2));
		}
	}
}