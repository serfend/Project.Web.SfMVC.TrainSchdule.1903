using System;
using System.Collections.Generic;
using System.Text;
using BLL.Interfaces;
using System.Globalization;
using System.Linq;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.UserInfo;
using BLL.Extensions;

namespace BLL.Services
{
	public class VocationCheckServices:IVocationCheckServices
	{
		private readonly ApplicationDbContext _context;
		public VocationCheckServices(ApplicationDbContext context)
		{
			_context = context;
		}
		/// <summary>
		/// 当时间为年初（1月1日0时0分0秒）时执行一次
		/// 重置所有人全年假期，并初始化年初假期
		/// </summary>
		public void ReloadNewYearVocation()
		{
			var allUsers = _context.AppUsers.ToList<User>();
			foreach(var u in allUsers)
			{
				if (u.Application.ApplicationSetting.LastVocationUpdateTime?.Year == DateTime.Today.Year) continue;
				u.SocialInfo.Settle.PrevYearlyLength = u.SocialInfo.Settle.GetYearlyLengthInner(u, out var i, out var j);
				u.Application.ApplicationSetting.LastVocationUpdateTime = DateTime.Today;
			}
		}

		public void AddDescription(VocationDescription model)
		{
			_context.VocationDescriptions.Add(model);
			 
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
			VocationDesc = GetVocationDescriptions(start, length);
			return EndDate;
		}

		public IEnumerable<VocationDescription> GetVocationDescriptions(DateTime start, int length)
		{
			if (length > 500) return null;
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
			VocationDesc = list;
			return list;
		}

		public DateTime EndDate { get; private set; }
		public IEnumerable<VocationDescription> VocationDesc { get; set; }
	}
}
