using BLL.Helpers;
using DAL.Entities.Vocations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Statistics
{
	public class NewStatisticsListViewModel:ApiResult
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
	public static class NewStatisticsExtensions
	{
		public static NewStatisticsSingleDataModel ToMode(this VocationStatisticsDescription model,VocationStatistics parent)
		{
			if (parent == null || model == null) return null;
			return new NewStatisticsSingleDataModel()
			{
				Id=model.StatisticsId,
				Start= parent.Start,
				End= parent.End,
				Description=parent.Description
			};
		}
	}
}
