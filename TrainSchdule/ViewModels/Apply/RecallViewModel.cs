using DAL.DTO.Recall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Apply
{
	public class RecallViewModel:ApiDataModel
	{
		public RecallOrderVDto Data { get; set; }
	}
}
