using DAL.Entities.UserInfo;
using DAL.Entities.ZX.Phy;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces.ZX
{
	public interface IPhyGradeServices
	{
		/// <summary>
		/// 根据用户情况获取其体能标准
		/// </summary>
		/// <param name="userBaseInfo"></param>
		/// <returns></returns>
		GradePhyStandard GetStandard(GradePhySubject subject, UserBaseInfo userBaseInfo);

		int GetGrade(GradePhyStandard standard, string rawValue);

		/// <summary>
		/// 根据科目名称以及用户情况筛选可选科目
		/// </summary>
		/// <param name="model">搜索条件,Names须指定Arrays</param>
		/// <param name="userBase"></param>
		/// <returns></returns>
		IEnumerable<GradePhySubject> GetSubjectsByName(QueryUserGradeViewModel model, UserBaseInfo userBase);

		void AddSubject(GradePhySubject model);

		GradePhySubject FindSubject(string name);
	}
}