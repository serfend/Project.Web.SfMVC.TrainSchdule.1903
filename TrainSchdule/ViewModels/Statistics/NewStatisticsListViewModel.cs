using BLL.Extensions;
using BLL.Helpers;
using DAL.DTO.Company;
using DAL.Entities.Vacations;
using System;
using System.Collections.Generic;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Statistics
{
	/// <summary>
	///
	/// </summary>
	public class NewStatisticsListViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public EntitiesListDataModel<NewStatisticsSingleDataModel> Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class NewStatisticsSingleDataModel
	{
		/// <summary>
		/// 通过id和统计初始化
		/// </summary>
		/// <param name="id"></param>
		/// <param name="parent"></param>
		public NewStatisticsSingleDataModel(string id, VacationStatistics parent)
		{
			Id = id;
			Start = parent?.Start ?? DateTime.MinValue;
			End = parent?.End ?? DateTime.MinValue;
			Description = parent?.Description;
		}

		/// <summary>
		///
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		///
		/// </summary>
		public DateTime Start { get; set; }

		/// <summary>
		///
		/// </summary>
		public DateTime End { get; set; }

		/// <summary>
		/// 通常自动生成，也可手动修改
		/// </summary>
		public string Description { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class CurrentLevelStatisticsDataModel
	{
		/// <summary>
		///
		/// </summary>
		public string StatisticsId { get; set; }

		/// <summary>
		/// 单位
		/// </summary>
		public virtual CompanyDto Company { get; set; }

		/// <summary>
		/// 单位本级数据
		/// </summary>
		public virtual VacationStatisticsData CurrentLevelStatistics { get; set; }

		/// <summary>
		/// 包含子单位数据
		/// </summary>
		public virtual VacationStatisticsData IncludeChildLevelStatistics { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class CurrentLevelStatisticsViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public CurrentLevelStatisticsDataModel Data { get; set; }
	}
}