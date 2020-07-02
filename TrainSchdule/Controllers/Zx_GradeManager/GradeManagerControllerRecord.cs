using BLL.Helpers;
using DAL.Entities;
using DAL.Entities.ZX.Phy;
using DAL.QueryModel.ZX;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.Controllers.Zx_GradeManager
{
	/// <summary>
	/// 用于记录成绩和考核
	/// </summary>
	public partial class GradeManagerController
	{
		/// <summary>
		/// 编辑一个成绩记录
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("Grade/Phy")]
		public IActionResult EditGrade([FromBody] EntityWithAuthDataModel<GradePhyRecord> model)
		{
			var prev = context.GradePhyRecords.Where(r => r.Id == model.Model.Id).FirstOrDefault();
			CheckPermission(model?.Auth, DictionaryAllPermission.Grade.Record, model.Model.GetOperation(prev), prev?.User?.CompanyInfo?.Company?.Code);
			phyGradeServices.ModifyRecord(model.Model);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 获取成绩记录
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("Grade/Phy")]
		public IActionResult GetGrade([FromBody] QueryGradeRecordViewModel model)
		{
			var result = phyGradeServices.GetRecords(model);
			return new JsonResult(new EntitiesListViewModel<GradePhyRecord>(result.Item1, result.Item2));
		}
	}
}