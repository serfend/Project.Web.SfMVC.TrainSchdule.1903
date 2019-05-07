using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.DTO.Apply;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Apply
{
	public class InfoApplyDetailViewModel : APIDataModel
	{
		public ApplyDetailDTO Data { get; set; }
	}

}
