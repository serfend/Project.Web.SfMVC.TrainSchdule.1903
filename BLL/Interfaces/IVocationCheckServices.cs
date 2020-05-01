using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace BLL.Interfaces
{
	public interface IVocationCheckServices
	{
		void AddDescription(VocationDescription model);

		/// <summary>
		/// 获取范围内所有的节假日
		/// </summary>
		/// <param name="targetDate"></param>
		/// <param name="length"></param>
		/// <param name="CheckInner">是否检查实际包含假期的长度 例如1.1-1.3元旦，从1.2-1.12只能算2天假期</param>
		/// <returns></returns>
		IEnumerable<VocationDescription> GetVocationDates(DateTime targetDate, int length, bool CheckInner);

		/// <summary>
		/// 判断从指定日期开始，跳过法定节假日会到何时
		/// </summary>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		Task<DateTime> CrossVocation(DateTime start, int length, bool caculateLawVocation);

		/// <summary>
		/// 获取从指定日期开始，跳过法定节假日包含的情况
		/// </summary>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		Task<IEnumerable<VocationDescription>> GetVocationDescriptions(DateTime start, int length, bool caculateLawVocation);

		DateTime EndDate { get; }
		IEnumerable<VocationDescription> VocationDesc { get; set; }
	}
}