﻿using DAL.Entities.Vocations;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
	public interface IVacationStatisticsServices
	{
		IEnumerable<VocationStatisticsDescription> Query(QueryVacationStatisticsDataModel model);
	}
}