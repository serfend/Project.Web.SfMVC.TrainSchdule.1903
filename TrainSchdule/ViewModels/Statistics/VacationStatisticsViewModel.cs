using BLL.Helpers;
using DAL.Entities.Vacations;
using System.Collections.Generic;

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
		public VacationStatisticsDescriptionsDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class VacationStatisticsDescriptionsDataModel
	{
		/// <summary>
		///
		/// </summary>
		public IEnumerable<VacationStatisticsDescription> List { get; set; }
	}
}