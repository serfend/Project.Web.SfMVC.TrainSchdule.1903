using BLL.Extensions;
using BLL.Helpers;
using DAL.DTO.Company;
using DAL.Entities.Vocations;
using System;
using System.Collections.Generic;

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
		public NewStatisticsListDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class NewStatisticsListDataModel
	{
		/// <summary>
		///
		/// </summary>
		public IEnumerable<NewStatisticsSingleDataModel> List { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class NewStatisticsSingleDataModel
	{
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
		public virtual VocationStatisticsData CurrentLevelStatistics { get; set; }

		/// <summary>
		/// 包含子单位数据
		/// </summary>
		public virtual VocationStatisticsData IncludeChildLevelStatistics { get; set; }
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

	/// <summary>
	///
	/// </summary>
	public static class NewStatisticsExtensions
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static CurrentLevelStatisticsDataModel ToDetailDataModel(this VacationStatisticsDescription model)
		{
			return new CurrentLevelStatisticsDataModel()
			{
				Company = model.Company.ToDto(),
				StatisticsId = model.StatisticsId,
				CurrentLevelStatistics = model.CurrentLevelStatistics,
				IncludeChildLevelStatistics = model.IncludeChildLevelStatistics
			};
		}

		/// <summary>
		/// 获取概略统计信息
		/// </summary>
		/// <param name="model"></param>
		/// <param name="parent"></param>
		/// <returns></returns>
		public static NewStatisticsSingleDataModel ToSummaryModel(this VacationStatisticsDescription model, VocationStatistics parent)
		{
			if (parent == null || model == null) return null;
			return new NewStatisticsSingleDataModel()
			{
				Id = model.StatisticsId,
				Start = parent.Start,
				End = parent.End,
				Description = parent.Description
			};
		}
	}
}