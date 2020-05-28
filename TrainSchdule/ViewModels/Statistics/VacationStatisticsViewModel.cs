using BLL.Helpers;
using DAL.Entities.Vacations;
using DAL.Entities.Vacations.VacationsStatistics;
using System.Collections.Generic;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Statistics
{
	/// <summary>
	/// 单个
	/// </summary>
	public class VacationStatisticsViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public VacationStatisticsDescription Data { get; set; }
	}

	/// <summary>
	/// 列表
	/// </summary>
	public class VacationStatisticsDescriptionsViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public EntitiesListDataModel<VacationStatisticsDescription> Data { get; set; }
	}
}