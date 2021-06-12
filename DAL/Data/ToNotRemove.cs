using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Data
{
	public static class ToNotRemove
	{
		public static IQueryable<T> ToExistQueryable<T>(this DbSet<T> dbSet) where T : BaseEntity
		{
			if (dbSet == null) return null;
			return dbSet.Where(d => !d.IsRemoved);
		}
	}
}