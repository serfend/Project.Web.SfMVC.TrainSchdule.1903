using BLL.Helpers;

namespace TrainSchdule.ViewModels.Statistics
{
	/// <summary>
	///
	/// </summary>
	public class DateWeekOfYearViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public DateWeekOfYearDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class DateWeekOfYearDataModel
	{
		/// <summary>
		///
		/// </summary>
		public int WeekOfYear { get; set; }
	}
}