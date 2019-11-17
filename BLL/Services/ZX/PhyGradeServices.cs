
using BLL.Interfaces.ZX;
using DAL.Data;
using DAL.Entities.UserInfo;
using DAL.Entities.ZX.Phy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL.Services.ZX
{
	public class PhyGradeServices : IPhyGradeServices
	{
		private readonly ApplicationDbContext _context;

		public PhyGradeServices(ApplicationDbContext context)
		{
			_context = context;
		}
		private int GetExpressionGrade(int value, string expression)
		{
			using (var d = new DataTable())
			{
				if (expression == null) return 0;
				var result = Convert.ToInt32(d.Compute(expression.Replace("x", value.ToString()), ""));
				return result;
			}
		}
		public int GetGrade(Standard standard, string rawValue)
		{
			if (standard == null) return 0;
			var value = standard.ToValue(rawValue);
			KeyValuePair<int,int> prev = new KeyValuePair<int, int>();
			foreach (var i in standard.GradePairsInner)
			{
				if (i.Key > value) return prev.Value;
				prev = i;
			}
			return GetExpressionGrade(value, standard.ExpressionWhenFullGrade);
		}

		public Standard GetStandard(Subject subject, UserBaseInfo userBaseInfo)
		{
			if (subject == null || userBaseInfo == null) return null;
			var userAge = userBaseInfo.Time_BirthDay.Age();
			subject.Standards = subject.Standards;
			foreach (var i in subject.Standards)
			{
				if (i.gender == userBaseInfo.Gender && i.minAge <= userAge && i.maxAge >= userAge) return i;
			}
			return null;
		}

		public Subject GetSubjectByName(string subjectName)
		{
			var r = _context.Subjects.Where(s => s.Name == subjectName).FirstOrDefault();
			if (r != null) r.Standards = r.Standards;
			return r;
		}

		public void AddSubject(Subject model)
		{
			if (model == null) return;
			var item = _context.Subjects.Where(s => s.Name == model.Name).FirstOrDefault();
			if (item != null) {
				_context.Standards.RemoveRange(item.Standards);
				_context.Subjects.Remove(item);
			}
			_context.Subjects.Add(model);
			_context.SaveChanges();
		}

		public Subject FindSubject(string name)
		{
			var r= _context.Subjects.Where(s => s.Name == name).FirstOrDefault();
			r.Standards=r.Standards;
			if (r == null)
			{
				var names = name.Split(' ');
				IQueryable<Subject> results = _context.Subjects ;
				IQueryable<Subject>  prev = null;
				foreach(var n in names)
				{
					results = results.Where(s => s.Name.Contains(n));
					if ((results.Count() > 1)) prev = results; else break;
				}
				return prev?.FirstOrDefault();
			}
			return r ;
		}
	}
}
