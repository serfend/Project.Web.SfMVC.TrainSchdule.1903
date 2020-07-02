using DAL.Entities.UserInfo;
using DAL.Entities.ZX.Phy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Extensions
{
	public static class UserGradeExtensions
	{
		public static IQueryable<GradePhySubject> GetSubjectsByUser(this IQueryable<GradePhySubject> list, UserBaseInfo userBase)
		{
			if (userBase == null) return list;
			var userAge = userBase.Time_BirthDay.Age();
			return list.Where(s => s.Standards.Any(d => d.minAge <= userAge && d.maxAge >= userAge && d.gender == userBase.Gender));
		}
	}
}