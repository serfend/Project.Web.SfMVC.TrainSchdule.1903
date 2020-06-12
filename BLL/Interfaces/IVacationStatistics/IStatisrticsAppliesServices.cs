using DAL.Entities.Vacations.Statistics.StatisticsNewApply;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces.IVacationStatistics
{
	/// <summary>
	/// 统计休假去向情况
	/// </summary>
	public interface IStatisrticsAppliesServices : IStatisticsComplete<StatisticsApplyComplete>, IStatisticsNew<StatisticsApplyNew>
	{
	}
}