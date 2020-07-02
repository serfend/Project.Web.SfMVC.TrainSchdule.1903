using BLL.Extensions.Common;
using BLL.Interfaces.ZX.IGrade;
using DAL.Data;
using DAL.Entities.ZX.Grade;
using DAL.QueryModel;
using DAL.QueryModel.ZX;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Services.ZX.Grade
{
	public partial class GradeServices : IGradeServices
	{
		private readonly ApplicationDbContext _context;

		public GradeServices(ApplicationDbContext context)
		{
			this._context = context;
		}

		public Tuple<IEnumerable<GradeExam>, int> GetExams(QueryGradeExamViewModel model)
		{
			if (model == null) return null;
			var result = _context.GradeExams.AsQueryable();
			if (model.CreateBy.Valid()) result = result.Where(r => r.CreateBy.Id == model.CreateBy.Value);
			if (model.HandleBy.Valid()) result = result.Where(r => r.HandleBy.Id == model.HandleBy.Value);
			if (model.HoldBy.Valid()) result = result.Where(r => r.HoldBy.Code == model.HoldBy.Value);
			var nameLen = model.Name.Value.Length;
			if (model.Name.Valid()) result = result.Where(r => r.Name.Length >= nameLen).Where(r => r.Name.Substring(0, nameLen) == model.Name.Value);

			if (!model.Create.Valid()) model.Create = new QueryByDate() { Start = DateTime.Now.AddYears(-1), End = DateTime.Now };
			if (model.ExecuteTime.Valid()) result = result.Where(r => r.ExecuteTime >= model.ExecuteTime.Start).Where(r => r.ExecuteTime <= model.ExecuteTime.End);

			result = result.Where(r => r.Create >= model.Create.Start).Where(r => r.Create <= model.Create.End);
			var list = result.SplitPage(model.Pages.ValidSplitPage()).Result;
			return new Tuple<IEnumerable<GradeExam>, int>(list.Item1.ToList(), list.Item2);
		}

		public GradeExam ModifyExam(GradeExam model)
		{
			var db = _context.GradeExams;
			var prev = db.Where(r => r.Name == model.Name).FirstOrDefault(); // use name as unique
			return model.Modify(db, prev, (m, p) =>
			{
				var newM = MapGradeExamModel(m);
				newM.CreateBy = p?.CreateBy ?? m.CreateBy;
				newM.Create = p?.Create ?? DateTime.Now;
				return newM;
			}, _context);
		}

		private GradeExam MapGradeExamModel(GradeExam model)
		{
			var createById = model.CreateBy.Id;
			model.CreateBy = _context.AppUsers.Where(u => u.Id == createById).FirstOrDefault();
			var holdById = model.HoldBy.Code;
			model.HoldBy = _context.Companies.Where(u => u.Code == holdById).FirstOrDefault();
			var handleById = model.HandleBy.Id;
			model.HandleBy = _context.AppUsers.Where(u => u.Id == handleById).FirstOrDefault();
			return model;
		}
	}
}