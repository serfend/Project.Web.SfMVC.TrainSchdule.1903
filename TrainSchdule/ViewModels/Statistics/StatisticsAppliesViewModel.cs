using BLL.Helpers;
using DAL.Entities.Vacations.Statistics.StatisticsNewApply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Statistics
{
	/// <summary>
	///
	/// </summary>
	public class StatisticsAppliesViewModel<T> : ApiResult where T : IStatisticsApplyBase
	{
		/// <summary>
		///
		/// </summary>
		public StatisticsAppliesDataModel<T> Data { get; set; }
	}

	/// <summary>
	/// 统计休假去向情况
	/// </summary>
	public class StatisticsAppliesDataModel<T> where T : IStatisticsApplyBase
	{
		/// <summary>
		/// 所有有关的记录
		/// </summary>
		public IEnumerable<T> List { get; set; }
	}
}