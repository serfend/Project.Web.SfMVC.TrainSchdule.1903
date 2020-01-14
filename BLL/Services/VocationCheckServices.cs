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
	public class VocationCheckServices : IVocationCheckServices
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
			foreach (var u in allUsers)
			{
				if (u.Application.ApplicationSetting.LastVocationUpdateTime?.Year == DateTime.Today.Year) continue;
				u.SocialInfo.Settle.PrevYearlyLength = u.SocialInfo.Settle.GetYearlyLengthInner(u, out var i, out var j);
				u.SocialInfo.Settle.PrevYearlyComsumeLength = _context.Applies.Where(a => a.BaseInfo.From.Id == u.Id && a.RequestInfo.StampLeave.Value.Year == DateTime.Today.Year - 1 && a.RequestInfo.VocationType == "事假").Sum(a => a.RequestInfo.VocationLength);//将去年休的事假记录
				u.Application.ApplicationSetting.LastVocationUpdateTime = DateTime.Today;
			}
		}
		public void AddDescription(VocationDescription model)
		{
			_context.VocationDescriptions.Add(model);
			_context.SaveChanges();
		}

		public IEnumerable<VocationDescription> GetVocationDates(DateTime date, int length)
		{
			var endDate = date.AddDays(length);
			return _context.VocationDescriptions.Where(v => v.Start < endDate)
				.Where(v => v.Start.AddDays(v.Length) >= date).ToList().Select(v => new VocationDescription()
				{
					Name = v.Name,
					Length=v.Length,
					Start = v.Start
				});
		}
		/// <summary>
		/// 当用户本应休假日期进入了假期范围，则应添加整个范围
		/// </summary>
		/// <param name="userNormalDateStart"></param>
		/// <param name="vocationStart"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		private int GetCrossDay(DateTime userNormalDateStart, DateTime userNormalDateEnd, DateTime vocationStart, DateTime vocationEnd)
		{
			if (userNormalDateStart <= vocationStart && userNormalDateEnd >= vocationStart) return vocationEnd.Subtract(vocationStart).Days;
			if (userNormalDateStart > vocationStart && userNormalDateStart <= vocationEnd) return vocationEnd.Subtract(userNormalDateStart).Days;
			return 0;
		}
		/// <summary>
		/// 判断日期经过一定天数后到达的日期
		/// </summary>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public DateTime CrossVocation(DateTime start, int length, bool caculateLawVocation)
		{
			VocationDesc = GetVocationDescriptions(start, length, caculateLawVocation);
			return EndDate;
		}

		public IEnumerable<VocationDescription> GetVocationDescriptions(DateTime start, int length, bool caculateLawVocation)
		{
			length -= 1;// 【注意】此处因计算天数需要向前减一天
			if (length > 500 || length < 0)
			{
				EndDate = start;
				return null;
			}
			var list = new List<VocationDescription>();
			var end = start.AddDays(length);
			int vocationDay = 0;
			if (caculateLawVocation)
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
