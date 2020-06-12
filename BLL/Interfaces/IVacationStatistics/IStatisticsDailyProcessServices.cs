using BLL.Services.VacationStatistics;
using DAL.Entities.Vacations.Statistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces.IVacationStatistics
{
	/// <summary>
	/// 统计自当年1月1日以来的各类完成情况
	/// </summary>
	public interface IStatisticsDailyProcessServices : IStatisticsComplete<StatisticsDailyProcessRate>
	{
	}
}