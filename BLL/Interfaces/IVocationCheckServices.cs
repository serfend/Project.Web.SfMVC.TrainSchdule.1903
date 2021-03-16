using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO.Apply;
using DAL.Entities;

namespace BLL.Interfaces
{
	public interface IVacationCheckServices
	{
		void AddDescription(VacationDescription model);

		/// <summary>
		/// 获取范围内所有的节假日
		/// </summary>
		/// <param name="targetDate"></param>
		/// <param name="length"></param>
		/// <param name="CheckInner">是否检查实际包含假期的长度 例如1.1-1.3元旦，从1.2-1.12只能算2天假期</param>
		/// <returns></returns>
		IEnumerable<VacationDescriptionDto> GetVacationDates(DateTime targetDate, int length, bool CheckInner);

		/// <summary>
		/// 判断从指定日期开始，跳过法定节假日会到何时
		/// </summary>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <param name="userSetList"></param>
		/// <returns></returns>
		DateTime CrossVacation(DateTime start, int length, bool caculateLawVacation, Dictionary<int, int> userSetList);

		/// <summary>
		/// 获取从指定日期开始，跳过法定节假日包含的情况
		/// </summary>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <param name="caculateLawVacation">是否计算法定节假日，不计算时，按简单的相加计算长度</param>
		/// <param name="userSetList">用户指定假期天数</param>
		/// <param name="exceptVacationCount">需要排除多少个假期（以避免重复计算）</param>
		/// <returns></returns>
		IEnumerable<VacationDescriptionDto> GetVacationDescriptions(DateTime start, int length, bool caculateLawVacation, Dictionary<int, int> userSetList, int exceptVacationCount = 0);

		DateTime EndDate { get; }
		IEnumerable<VacationDescriptionDto> VacationDesc { get; set; }
	}
}