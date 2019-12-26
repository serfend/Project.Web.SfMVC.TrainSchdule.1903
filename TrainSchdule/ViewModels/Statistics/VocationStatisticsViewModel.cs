using BLL.Helpers;
using DAL.Entities.Vocations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Statistics
{
	public class VocationStatisticsViewModel:ApiResult
	{
		public VocationStatisticsDescription Data { get; set; }
	}
}
