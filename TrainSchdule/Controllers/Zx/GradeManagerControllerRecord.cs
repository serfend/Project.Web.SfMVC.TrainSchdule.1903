﻿using BLL.Helpers;
using DAL.Entities;
using DAL.Entities.Permisstions;
using DAL.Entities.ZX.Phy;
using DAL.QueryModel.ZX;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.ZX;

namespace TrainSchdule.Controllers.Zx
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
		[Route("Grade/Record")]
		public IActionResult EditGrade([FromBody] GradeRecordModifyViewModel model)
		{
			var prev = context.GradePhyRecords.Where(r => r.Id == model.Id).FirstOrDefault();
			var m = model.ToModel(context);
			// check exam holder modify
			var prevExamHolder = prev?.Exam?.HoldBy?.Code;
			var currentExamHolder = m?.Exam?.HoldBy?.Code;
			if (prevExamHolder != null && prevExamHolder != currentExamHolder)
				CheckPermission(model?.Auth, ApplicationPermissions.Grade.Subject.Record.Item, PermissionType.Write, prevExamHolder, "所属考核-移出原单位");
			CheckPermission(model?.Auth, ApplicationPermissions.Grade.Subject.Record.Item, PermissionType.Write, currentExamHolder, "所属考核");

			// check target user modify
			var prevUser = prev?.User?.CompanyInfo?.CompanyCode;
			var currentUser = m?.User?.CompanyInfo?.CompanyCode;
			if (prevUser != currentUser)
				CheckPermission(model?.Auth, ApplicationPermissions.Grade.Subject.Record.Item, PermissionType.Write, prevUser, "移出作用于成员");
			CheckPermission(model?.Auth, ApplicationPermissions.Grade.Subject.Record.Item, PermissionType.Write, currentUser, "作用于成员");

			phyGradeServices.ModifyRecord(m);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 获取成绩记录
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("Grade/Records")]
		public IActionResult GetGrade([FromBody] QueryGradeRecordViewModel model)
		{
			var result = phyGradeServices.GetRecords(model);
			return new JsonResult(new EntitiesListViewModel<GradePhyRecord>(result?.Item1, result?.Item2 ?? -1));
		}
	}
}