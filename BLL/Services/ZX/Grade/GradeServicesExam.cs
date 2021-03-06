using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces.ZX.IGrade;
using DAL.Data;
using DAL.DTO.ZX.Grade;
using DAL.Entities.ZX.Grade;
using DAL.QueryModel;
using DAL.QueryModel.ZX;
using Microsoft.EntityFrameworkCore;
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

		public Tuple<IEnumerable<ExamDTO>, int> GetExams(QueryGradeExamViewModel model)
		{
			if (model == null) return null;
			var result = _context.GradeExams.AsQueryable();
			if (model.CreateBy.Valid()) result = result.Where(r => r.CreateById == model.CreateBy.Value);
			if (model.HandleBy.Valid()) result = result.Where(r => r.HandleById == model.HandleBy.Value);
			if (model.HoldBy.Valid()) result = result.Where(r => r.HoldBy.Code == model.HoldBy.Value);
			if (model.Name.Valid())
			{
				var nameLen = model.Name.Value.Length;
				result = result.Where(r => r.Name.Length >= nameLen).Where(r => r.Name.StartsWith(model.Name.Value));
			}

			if (!model.Create.Valid()) model.Create = new QueryByDate() { Start = DateTime.Now.AddYears(-1), End = DateTime.Now };
			if (model.ExecuteTime.Valid()) result = result.Where(r => r.ExecuteTime >= model.ExecuteTime.Start).Where(r => r.ExecuteTime <= model.ExecuteTime.End);

			result = result.Where(r => r.Create >= model.Create.Start).Where(r => r.Create <= model.Create.End);
			var list = result.SplitPage(model.Pages.ValidSplitPage());
			return new Tuple<IEnumerable<ExamDTO>, int>(list.Item1.ToList().Select(f => f.ToDTO()), list.Item2);
		}

		public GradeExam ModifyExam(GradeExam model)
		{
			if (model == null) throw new ActionStatusMessageException(model.NotExist());
			var db = _context.GradeExams;
			if (model.HoldBy == null) throw new ActionStatusMessageException(ActionStatusMessage.Grade.Exam.UserNotSet);
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
			var createById = model.CreateById;
			if (createById != null) model.CreateBy = _context.AppUsersDb.Where(u => u.Id == createById).FirstOrDefault();
			var holdById = model.HoldBy?.Code;
			if (holdById != null) model.HoldBy = _context.CompaniesDb.Where(u => u.Code == holdById).FirstOrDefault();
			var handleById = model.HandleBy?.Id;
			if (handleById != null) model.HandleBy = _context.AppUsersDb.Where(u => u.Id == handleById).FirstOrDefault();
			return model;
		}
	}
}