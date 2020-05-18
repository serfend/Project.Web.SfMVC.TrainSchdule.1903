using DAL.Entities.Vacations;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
	public interface IVacationStatisticsServices
	{
		IEnumerable<VacationStatisticsDescription> Query(QueryVacationStatisticsViewModel model);
	}
}
