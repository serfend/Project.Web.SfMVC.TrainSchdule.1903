﻿using System;
using System.Collections.Generic;
using System.Text;
using BLL.Interfaces;
using System.Globalization;
using System.Linq;
using DAL.Data;
using DAL.Entities;

namespace BLL.Services
{
	public class VocationCheckServices:IVocationCheckServices
	{
		private readonly ApplicationDbContext _context;
		public VocationCheckServices(ApplicationDbContext context)
		{
			_context = context;
		}


		public void AddDescription(VocationDescription model)
		{
			_context.VocationDescriptions.Add(model);
			_context.SaveChanges();
		}

		public IEnumerable<VocationDescription> GetVocationDates(DateTime date,int length)
		{
			var endDate = date.AddDays(length);
			return _context.VocationDescriptions.Where(v => v.Start <= endDate)
				.Where(v => v.Start.AddDays(v.Length) >= date).ToList();
		}

		private int GetCrossDay(DateTime a1, DateTime a2, DateTime b1, DateTime b2)
		{
			var later = a1 > b1 ? a1 : b1;
			return b2.Subtract(later).Days;
		}
		public DateTime CrossVocation(DateTime start, int length)
		{
			var list=new List<VocationDescription>();
			var end = start.AddDays(length);
			int vocationDay = 0;
			foreach (var description in GetVocationDates(start,length))
			{
				description.Length = GetCrossDay(start, end, description.Start, description.Start.AddDays(description.Length));
				list.Add(description);
				vocationDay += description.Length;
			}

			EndDate = end.AddDays(vocationDay);
			return EndDate;
		}

		public IEnumerable<VocationDescription> GetVocationDescriptions(DateTime start, int length)
		{
			var list = new List<VocationDescription>();
			var end = start.AddDays(length);
			int vocationDay = 0;
			foreach (var description in GetVocationDates(start, length))
			{
				description.Length = GetCrossDay(start, end, description.Start, description.Start.AddDays(description.Length));
				list.Add(description);
				vocationDay += description.Length;
			}
			EndDate = end.AddDays(vocationDay);

			return list;
		}

		public DateTime EndDate { get; private set; }
	}
}