using DAL.Entities.Vacations;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces.IVacationStatistics
{
	/// <summary>
	/// 统计休假完成情况
	/// </summary>
	public interface IStatisticsAppliesProcessServices : IStatisticsComplete<StatisticsAppliesProcess>
	{
	}
}