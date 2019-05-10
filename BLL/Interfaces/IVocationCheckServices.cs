using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;

namespace BLL.Interfaces
{
	public interface IVocationCheckServices
	{
		void AddDescription(VocationDescription model);
		/// <summary>
		/// 获取范围内所有的节假日
		/// </summary>
		/// <param name="date"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		IEnumerable<VocationDescription> GetVocationDates(DateTime date,int length);
		/// <summary>
		/// 判断从指定日期开始，跳过法定节假日会到何时
		/// </summary>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		DateTime CrossVocation(DateTime start, int length);
		/// <summary>
		/// 获取从指定日期开始，跳过法定节假日包含的情况
		/// </summary>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		IEnumerable<VocationDescription> GetVocationDescriptions(DateTime start,int length);
		DateTime EndDate { get; }
	}
}
