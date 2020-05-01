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
using System.Threading.Tasks;

namespace BLL.Services
{
	public class VocationCheckServices : IVocationCheckServices
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

		/// <summary>
		/// 获取指定日期间包含的假期
		/// </summary>
		/// <param name="date"></param>
		/// <param name="length"></param>
		/// <param name="CheckInner">是否检查实际包含假期的长度 例如1.1-1.3元旦，从1.2-1.12只能算2天假期</param>
		/// <returns></returns>
		public IEnumerable<VocationDescription> GetVocationDates(DateTime date, int length, bool CheckInner)
		{
			var endDate = date.AddDays(length);
			return _context.VocationDescriptions.Where(v => v.Start <= endDate)
				.Where(v => v.Start.AddDays(v.Length) >= date).ToList().Select(v => new VocationDescription()
				{
					Name = v.Name,
					Length = CheckInner ? GetCrossDay(date, date.AddDays(length), v.Start, v.Start.AddDays(v.Length)) : v.Length,
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
		public async Task<DateTime> CrossVocation(DateTime start, int length, bool caculateLawVocation)
		{
			VocationDesc = await GetVocationDescriptions(start, length, caculateLawVocation).ConfigureAwait(true);
			return EndDate;
		}

		/// <summary>
		/// 判断并初始化指定日期范围内包含的假期长度
		/// </summary>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <param name="caculateLawVocation">是否计算法定节假日，不计算时，按简单的相加计算长度</param>
		/// <returns></returns>
		public async Task<IEnumerable<VocationDescription>> GetVocationDescriptions(DateTime start, int length, bool caculateLawVocation)
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
			await Task.Run(() =>
			{
				if (caculateLawVocation)
					foreach (var description in GetVocationDates(start, length, true))
					{
						list.Add(description);
						vocationDay += description.Length;
					}
			}).ConfigureAwait(true);

			EndDate = end.AddDays(vocationDay);
			VocationDesc = list;
			return list;
		}

		public DateTime EndDate { get; private set; }
		public IEnumerable<VocationDescription> VocationDesc { get; set; }
	}
}