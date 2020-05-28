using DAL.Entities.Vacations;
using DAL.Entities.Vacations.VacationsStatistics;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
	public interface IVacationStatisticsServices
	{
		Tuple<IEnumerable<VacationStatisticsDescription>, int> Query(QueryVacationStatisticsViewModel model);
	}
}