using BLL.Extensions;
using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces.ZX;
using DAL.Data;
using DAL.Entities.UserInfo;
using DAL.Entities.ZX.Phy;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL.Services.ZX
{
	public partial class PhyGradeServices : IPhyGradeServices
	{
		private readonly ApplicationDbContext _context;

		public PhyGradeServices(ApplicationDbContext context)
		{
			_context = context;
		}

		#region Subject&Standard

		private static int GetExpressionGrade(int value, int prevValue, string expression)
		{
			using (var d = new DataTable())
			{
				if (expression == null) return 0;
				try
				{
					var replaceStr = expression.ToLower().Replace("x", value.ToString()).Replace("y", prevValue.ToString());
					var result = Math.Floor(Convert.ToDouble(d.Compute(replaceStr, "")) + prevValue);
					return (int)result;
				}
				catch (Exception ex)
				{
					throw new ActionStatusMessageException(new ApiResult(ActionStatusMessage.Grade.Compute.UnExpected, ex.Message, true));
				}
			}
		}

		public int GetGrade(GradePhyStandard standard, string rawValue)
		{
			if (standard == null) return 0;
			var value = standard.ToValue(rawValue);
			if (value == -1) return 0;
			KeyValuePair<int, int> prev = new KeyValuePair<int, int>();
			foreach (var i in standard.GradePairsInner)
			{
				if (i.Key > value) return prev.Value;
				prev = i;
			}
			if (prev.Key >= value) return prev.Value;
			return GetExpressionGrade(value - prev.Key, prev.Value, standard.ExpressionWhenFullGrade);
		}

		public GradePhyStandard GetStandard(GradePhySubject subject, UserBaseInfo userBaseInfo)
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

		public IEnumerable<GradePhySubject> GetSubjectsByName(QueryUserGradeViewModel model, UserBaseInfo userBase, QueryByPage pages)
		{
			if (model == null) model = new QueryUserGradeViewModel();
			var has = model.Names?.Arrays?.Any() ?? false;
			if (!has) model.Names = new QueryByString() { Arrays = new List<string>() { "*" } };
			var group = model.Groups?.Value;
			var subjectN = model.Names.Arrays;
			var list = _context.GradePhySubjects.AsQueryable();
			var result = new List<GradePhySubject>();
			foreach (var sn in subjectN)
			{
				if (sn != "*") list = list.Where(s => s.Name == sn);
				if (group != null) list = list.Where(s => s.Group == group);
				var r = list.GetSubjectsByUser(userBase).SplitPage(pages);
				result.AddRange(r.Item1);
			}
			return result;
		}

		public void ModifySubject(GradePhySubject model)
		{
			if (model == null) return;
			var item = _context.GradePhySubjects.Where(s => s.Name == model.Name).FirstOrDefault();
			var existed = item != null;
			if (model.IsRemoved && !existed)
				throw new ActionStatusMessageException(ActionStatusMessage.Grade.Subject.NotExist);
			if (existed)
			{
				_context.GradePhySubjects.Remove(item);
				_context.GradePhyStandards.RemoveRange(item.Standards);
			}
			if (!model.IsRemoved)
				_context.GradePhySubjects.Add(model);
			_context.SaveChanges();
		}

		public GradePhySubject FindSubject(string name)
		{
			if (name == null) return null;
			var r = _context.GradePhySubjects.Where(s => s.Name == name).FirstOrDefault();

			if (r == null)
			{
				var names = name.Split(' ');
				IQueryable<GradePhySubject> results = _context.GradePhySubjects;
				IQueryable<GradePhySubject> prev = null;
				foreach (var n in names)
				{
					results = results.Where(s => s.Name.Contains(n));
					if ((results.Count() > 1)) prev = results; else break;
				}
				r = prev?.FirstOrDefault();
			}
			if (r == null) return null;
			r.Standards = r.Standards;
			return r;
		}

		#endregion Subject&Standard
	}
}