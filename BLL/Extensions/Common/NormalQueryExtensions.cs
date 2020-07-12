using BLL.Helpers;
using DAL.Data;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Extensions.Common
{
	public static class NormalQueryExtensions
	{
		public static T Modify<T>(this T model, DbSet<T> db, T prev, Func<T, T, T> MapModel, ApplicationDbContext context) where T : BaseEntity
		{
			if (model == null) return null;
			var existed = prev != null;
			if (model.IsRemoved && !existed) throw new ActionStatusMessageException(model.NotExist());
			// soft remove
			else if (existed && model.IsRemoved)
			{
				prev.Remove();
				db.Update(prev);
			}
			model = MapModel?.Invoke(model, prev);
			db.Update(model);
			context.SaveChanges();
			return model;
		}
	}
}