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
	public class VacationCheckServices : IVacationCheckServices
	{
		private readonly ApplicationDbContext _context;

		public VacationCheckServices(ApplicationDbContext context)
		{
			_context = context;
		}

		public void AddDescription(VacationDescription model)
		{
			_context.VacationDescriptions.Add(model);
			_context.SaveChanges();
		}

		/// <summary>
		/// 获取指定日期间包含的假期
		/// </summary>
		/// <param name="date"></param>
		/// <param name="length"></param>
		/// <param name="CheckInner">是否检查实际包含假期的长度 例如1.1-1.3元旦，从1.2-1.12只能算2天假期</param>
		/// <returns></returns>
		public IEnumerable<VacationDescription> GetVacationDates(DateTime date, int length, bool CheckInner)
		{
			var endDate = date.AddDays(length);
			return _context.VacationDescriptions.Where(v => v.Start <= endDate)
				.Where(v => v.Start.AddDays(v.Length) >= date).ToList().Select(v => new VacationDescription()
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
		/// <param name="vacationStart"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		private static int GetCrossDay(DateTime userNormalDateStart, DateTime userNormalDateEnd, DateTime vacationStart, DateTime vacationEnd)
		{
			if (userNormalDateStart <= vacationStart && userNormalDateEnd >= vacationStart) return vacationEnd.Subtract(vacationStart).Days;
			if (userNormalDateStart > vacationStart && userNormalDateStart <= vacationEnd) return vacationEnd.Subtract(userNormalDateStart).Days;
			return 0;
		}

		/// <summary>
		/// 判断日期经过一定天数后到达的日期
		/// </summary>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public async Task<DateTime> CrossVacation(DateTime start, int length, bool caculateLawVacation)
		{
			await GetVacationDescriptions(start, length, caculateLawVacation).ConfigureAwait(false);
			return EndDate;
		}

		/// <summary>
		/// 判断并初始化指定日期范围内包含的假期长度
		/// </summary>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <param name="caculateLawVacation">是否计算法定节假日，不计算时，按简单的相加计算长度</param>
		/// <returns></returns>
		public async Task<IEnumerable<VacationDescription>> GetVacationDescriptions(DateTime start, int length, bool caculateLawVacation)
		{
			length -= 1;// 【注意】此处因计算天数需要向前减一天
			if (length > 1000 || length < 0)
			{
				EndDate = start;
				return null;
			}
			var list = new List<VacationDescription>();
			var end = start.AddDays(length);
			int vacationDay = 0;
			await Task.Run(() =>
			{
				if (caculateLawVacation)
					foreach (var description in GetVacationDates(start, length, true))
					{
						list.Add(description);
						vacationDay += description.Length;
					}
			}).ConfigureAwait(true);

			EndDate = end.AddDays(vacationDay);
			VacationDesc = list;
			return list;
		}

		public DateTime EndDate { get; private set; }
		public IEnumerable<VacationDescription> VacationDesc { get; set; } = new List<VacationDescription>();
	}
}