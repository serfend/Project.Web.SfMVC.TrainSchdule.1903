using System.Collections.Generic;
using DAL.DTO.Apply;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Apply
{
	public class ApplyListViewModel:APIDataModel
	{
		public ApplyListDataModel Data { get; set; }
	}

	public class ApplyListDataModel
	{
		public IEnumerable<ApplySummaryDto> List { get; set; }
	}
}
