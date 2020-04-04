using BLL.Helpers;
using DAL.Entities.Vocations;
using System.Collections.Generic;

namespace TrainSchdule.ViewModels.Statistics
{
	/// <summary>
	///
	/// </summary>
	public class VocationStatisticsViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public VocationStatisticsDescription Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class VocationStatisticsDescriptions : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public IEnumerable<VocationStatisticsDescription> List { get; set; }
	}
}