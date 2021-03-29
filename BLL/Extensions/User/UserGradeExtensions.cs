using DAL.Entities.UserInfo;
using DAL.Entities.ZX.Phy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Extensions
{
    public interface IUserGradeBaseInfo
    {
        public DateTime Time_BirthDay { get; set; }
        public GenderEnum Gender { get; set; }
    }
	public class UserGradeBaseInfo : IUserGradeBaseInfo
    {
		public DateTime Time_BirthDay { get; set; }
		public GenderEnum Gender { get; set; }
	}
	public static class UserGradeExtensions
	{
		public static IQueryable<GradePhySubject> GetSubjectsByUser<T>(this IQueryable<GradePhySubject> list, T userBase)where T: IUserGradeBaseInfo
		{
			if (userBase == null) return list;
			var userAge = userBase.Time_BirthDay.Age();
			return list.Where(s => s.Standards.Any(d => d.minAge <= userAge && d.maxAge >= userAge && d.gender == userBase.Gender));
		}
	}
}