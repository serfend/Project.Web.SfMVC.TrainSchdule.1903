using DAL.Data;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using DAL.Entities.Vacations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Extensions
{
	public static class VacationStatisticsExtensions
	{
		public static void StatisticsInit(this VacationStatisticsDescription model, ApplicationDbContext context, int currentYear, string statisticsId)
		{
			if (context == null) return;
			if (model == null) return;
			model.StatisticsId = statisticsId;
			foreach (var m in model.Childs)
				m.StatisticsInit(context, currentYear, statisticsId);
		}
	}
}