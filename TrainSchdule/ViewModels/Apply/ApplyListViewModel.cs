using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.DTO.Apply;

namespace TrainSchdule.ViewModels.Apply
{
	public class ApplyListViewModel:APIDataModel
	{
		public ApplyListDataModel Data { get; set; }
	}

	public class ApplyListDataModel
	{
		public IEnumerable<ApplySummaryDTO> List { get; set; }
	}
}
