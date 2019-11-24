using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Statistics
{
	/// <summary>
	/// 
	/// </summary>
	public class DateWeekOfYearViewModel:ApiDataModel
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
