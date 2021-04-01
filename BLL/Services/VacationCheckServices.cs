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
using DAL.DTO.Apply;
using Microsoft.EntityFrameworkCore;

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
		public IEnumerable<VacationDescriptionDto> GetVacationDates(DateTime date, int length, bool CheckInner)
		{
			var endDate = date.AddDays(length);
			return _context.VacationDescriptions.AsNoTracking().Where(v => v.Start <= endDate)
				.Where(v => v.Start.AddDays(v.Length) >= date).ToList().Select(v => {
					int finnal_length = CheckInner ? GetCrossDay(date, date.AddDays(length), v.Start, v.Start.AddDays(v.Length)) :v.Length;
					return v.ToModel(finnal_length);
				}) ;
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
		/// <param name="userSetList"></param>
		/// <returns></returns>
		public DateTime CrossVacation(DateTime start, int length, bool caculateLawVacation, Dictionary<int, int> userSetList)
		{
			GetVacationDescriptions(start, length, caculateLawVacation, userSetList);
			return EndDate;
		}

		/// <summary>
		/// 判断并初始化指定日期范围内包含的假期长度
		/// </summary>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <param name="caculateLawVacation">是否计算法定节假日，不计算时，按简单的相加计算长度</param>
		/// <param name="userSetList">用户指定的假期长度</param>
		/// <param name="exceptVacationCount">需要排除多少个假期（以避免重复计算）</param>
		/// <returns></returns>
		public IEnumerable<VacationDescriptionDto> GetVacationDescriptions(DateTime start, int length, bool caculateLawVacation,Dictionary<int,int> userSetList, int exceptVacationCount = 0)
		{
			// 初始化
			if (exceptVacationCount < 0) exceptVacationCount = 0;
			if (exceptVacationCount == 0) VacationDesc = new List<VacationDescriptionDto>();
			length -= 1;// 【注意】此处因计算天数需要向前减一天
			if (length > 1000 || length < 0)
			{
				EndDate = start;
				return null;
			}
			var list = new List<VacationDescriptionDto>();
			var end = start.AddDays(length);
			int vacationCount = 0;
			int vacationDay = 0;
			if (caculateLawVacation)
			{
				var vas = GetVacationDates(start, length, true)
					.ToList();
				vacationCount = vas.Count;
				for (var i = exceptVacationCount; i < vas.Count; i++)
				{
					var description = vas[i];
					var userSet = userSetList.ContainsKey(description.Id)? userSetList[description.Id]:description.Length;
					list.Add(description.AttachUserSet(userSet));
					vacationDay += description.UseLength;
				}
			}
			VacationDesc = VacationDesc.Concat(list);
			// 如果本轮计算了假期，则结果可能因为计算的假期而达到新假期的标准
			// 此时应从开始日期重新计算包含假期后的实际长度，并加上新的假期
			if (vacationCount > exceptVacationCount) return GetVacationDescriptions(start, vacationDay + length + 1, true, userSetList, vacationCount);
			end = end.AddDays(vacationDay);
			EndDate = end;
			return VacationDesc;
		}

		public DateTime EndDate { get; private set; }
		public IEnumerable<VacationDescriptionDto> VacationDesc { get; set; } = new List<VacationDescriptionDto>();
	}
}