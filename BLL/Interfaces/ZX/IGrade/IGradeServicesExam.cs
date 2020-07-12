using DAL.DTO.ZX.Grade;
using DAL.Entities.ZX.Grade;
using DAL.QueryModel.ZX;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces.ZX.IGrade
{
	public partial interface IGradeServices
	{
		/// <summary>
		/// 新增/更新/删除 考核
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		GradeExam ModifyExam(GradeExam model);

		/// <summary>
		/// 查询指定考核
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Tuple<IEnumerable<ExamDTO>, int> GetExams(QueryGradeExamViewModel model);
	}
}