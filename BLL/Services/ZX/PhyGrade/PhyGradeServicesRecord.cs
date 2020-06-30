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
		public GradePhyRecord Modify(GradePhyRecord model)
		{
			if (model == null) return null;
			var db = _context.GradePhyRecords;
			var prev = db.Where(r => r.Id == model.Id).FirstOrDefault();
			var existed = prev != null;
			if (model.IsRemoved && !existed) throw new ActionStatusMessageException(ActionStatusMessage.Grade.Record.NotExist);
			if (existed) db.Remove(prev);
			if (!model.IsRemoved) db.Add(model);
			_context.SaveChanges();
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
			var list = result.SplitPage(model.Pages.ValidSplitPage()).Result;
			return new Tuple<IEnumerable<GradePhyRecord>, int>(list.Item1.ToList(), list.Item2);
		}
	}
}