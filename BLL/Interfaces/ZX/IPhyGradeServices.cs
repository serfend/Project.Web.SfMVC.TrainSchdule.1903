using DAL.Entities.UserInfo;
using DAL.Entities.ZX.Phy;
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
		Standard GetStandard(Subject subject, UserBaseInfo userBaseInfo);
		int GetGrade(Standard standard, string rawValue);
		/// <summary>
		/// 根据科目名称以及用户情况筛选可选科目
		/// </summary>
		/// <param name="subjectName">可模糊搜搜，以【 】分割，搜索多个，以【|】分割</param>
		/// <param name="userBase"></param>
		/// <returns></returns>
		Subject GetSubjectByName(string subjectName,UserBaseInfo userBase);
		void AddSubject(Subject model);
		Subject FindSubject(string name);
	}
}
