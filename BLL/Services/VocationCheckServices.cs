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
				u.SocialInfo.Settle.PrevYearlyComsumeLength = _context.Applies.Where(a => a.BaseInfo.From.Id == u.Id&&a.RequestInfo.StampLeave.Value.Year==DateTime.Today.Year-1&&a.RequestInfo.VocationType=="事假").Sum(a=>a.RequestInfo.VocationLength);//将去年休的事假记录
				u.Application.ApplicationSetting.LastVocationUpdateTime = DateTime.Today;
			}
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
		/// <summary>
		/// 判断两个日期之间交叉的天数
		/// </summary>
		/// <param name="d1Start"></param>
		/// <param name="d1End"></param>
		/// <param name="d2Start"></param>
		/// <param name="d2End"></param>
		/// <returns></returns>
		private int GetCrossDay(DateTime d1Start, DateTime d1End, DateTime d2Start, DateTime d2End)
		{
			var later = d1Start > d2Start ? d1Start : d2Start;
			var early = d1End > d2End ? d2End : d1End;
			var result= early.Subtract(later).Days;
			return result < 0 ? 0 : result;
		}
		/// <summary>
		/// 判断日期经过一定天数后到达的日期
		/// </summary>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public DateTime CrossVocation(DateTime start, int length)
		{
			VocationDesc = GetVocationDescriptions(start, length);
			return EndDate;
		}

		public IEnumerable<VocationDescription> GetVocationDescriptions(DateTime start, int length)
		{
			if (length > 500) return null;
			var list = new List<VocationDescription>();
			var end = start.AddDays(length-1);//此处定义休假1天为从当天早上到晚上，故实际天数-1
			int vocationDay = 0;
			foreach (var description in GetVocationDates(start, length-1))
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
