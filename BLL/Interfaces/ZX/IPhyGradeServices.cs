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
		Subject GetSubjectByName(string subjectName);
		void AddSubject(Subject model);
		Subject FindSubject(string name);
	}
}
