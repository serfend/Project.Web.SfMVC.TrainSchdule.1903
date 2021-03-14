using BLL.Helpers;
using DAL.DTO.ZX.Grade;
using DAL.Entities;
using DAL.Entities.ZX.Grade;
using DAL.QueryModel.ZX;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.ZX;

namespace TrainSchdule.Controllers.Zx
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
			var m = model.ToModel(context);
			var prev = context.GradeExams.Where(e => e.Name == m.Name).FirstOrDefault();
			var operation = m.GetOperation(prev);
			if (prev?.HoldBy != null && prev.HoldBy?.Code != m?.HoldBy?.Code) CheckPermission(model?.Auth, DictionaryAllPermission.Grade.Exam, operation, prev.HoldBy.Code, "移出原单位");
			var authUser = CheckPermission(model?.Auth, DictionaryAllPermission.Grade.Exam, operation, m.HoldBy.Code);
			m.CreateBy = authUser;
			gradeServices.ModifyExam(m);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 获取考核记录
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("Grade/Exams")]
		public IActionResult GetExam([FromBody] QueryGradeExamViewModel model)
		{
			var result = gradeServices.GetExams(model);
			return new JsonResult(new EntitiesListViewModel<ExamDTO>(result?.Item1, result?.Item2 ?? -1));
		}
	}
}