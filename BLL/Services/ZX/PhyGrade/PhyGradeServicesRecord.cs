using BLL.Extensions;
using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces.ZX;
using DAL.Data;
using DAL.Entities.UserInfo;
using DAL.Entities.ZX.Phy;
using DAL.QueryModel;
using DAL.QueryModel.ZX;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL.Services.ZX
{
	public partial class PhyGradeServices
	{
		public GradePhyRecord ModifyRecord(GradePhyRecord model)
		{
			if (model == null) throw new ActionStatusMessageException(model.NotExist());
			var db = _context.GradePhyRecords;
			var prev = db.Where(r => r.Id == model.Id).FirstOrDefault();
			if (model.User == null) throw new ActionStatusMessageException(ActionStatusMessage.Grade.Record.UserNotSet);
			return model.Modify(db, prev, (m, p) =>
		   {
			   var newM = MapPhyRecordModel(m);
			   newM.CreateBy = p?.CreateBy ?? m.CreateBy;
			   newM.Create = p?.Create ?? DateTime.Now;
			   return newM;
		   }, _context);
		}

		private GradePhyRecord MapPhyRecordModel(GradePhyRecord model)
		{
			var createById = model.CreateBy.Id;
			model.CreateBy = _context.AppUsersDb.Where(u => u.Id == createById).FirstOrDefault();
			var examId = model.Exam.Id;
			model.Exam = _context.GradeExams.Where(e => e.Id == examId).FirstOrDefault();
			var userId = model.User.Id;
			model.User = _context.AppUsersDb.Where(u => u.Id == userId).FirstOrDefault();
			var subjectId = model.Subject.Id;
			model.Subject = _context.GradePhySubjects.Where(s => s.Id == subjectId).FirstOrDefault();
			return model;
		}

		public Tuple<IEnumerable<GradePhyRecord>, int> GetRecords(QueryGradeRecordViewModel model)
		{
			if (model == null) return null;
			var result = _context.GradePhyRecords.AsQueryable();
			if (model.CreateBy.Valid()) result = result.Where(r => r.CreateBy.Id == model.CreateBy.Value);
			if (model.CreateFor.Valid()) result = result.Where(r => r.User.Id == model.CreateFor.Value);
			if (!model.Create.Valid()) model.Create = new QueryByDate() { Start = DateTime.Now.AddYears(-1), End = DateTime.Now };
			result = result.Where(r => r.Create >= model.Create.Start).Where(r => r.Create <= model.Create.End);
			var list = result.SplitPage(model.Pages.ValidSplitPage());
			return new Tuple<IEnumerable<GradePhyRecord>, int>(list.Item1.ToList(), list.Item2);
		}
	}
}