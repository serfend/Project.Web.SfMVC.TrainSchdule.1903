using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.DTO.Company;
using DAL.Entities.Vocations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Statistics
{
	public class NewStatisticsListViewModel : ApiResult
	{
		public NewStatisticsListDataModel Data { get; set; }
	}
	public class NewStatisticsListDataModel
	{
		public IEnumerable<NewStatisticsSingleDataModel> List { get; set; }
	}
	public class NewStatisticsSingleDataModel
	{
		public string Id { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		/// <summary>
		/// 通常自动生成，也可手动修改
		/// </summary>
		public string Description { get; set; }
	}
	public class CurrentLevelStatisticsDataModel
	{
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
	public class CurrentLevelStatisticsViewModel : ApiResult
	{
		public CurrentLevelStatisticsDataModel Data { get; set; }
	}
	public static class NewStatisticsExtensions
	{
		public static CurrentLevelStatisticsDataModel ToDetailDataModel(this VocationStatisticsDescription model)
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
		public static NewStatisticsSingleDataModel ToSummaryModel(this VocationStatisticsDescription model, VocationStatistics parent)
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
