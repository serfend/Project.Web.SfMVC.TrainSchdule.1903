using BLL.Helpers;
using DAL.Entities.ZX.Phy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.ZX
{
	public class PhySubjectViewModel:ApiResult
	{
		public Subject Data { get; set; }
	}
	public class PhySubjectDataModel: GoogleAuthViewModel
	{
		public Subject Subject { get; set; }
	}
}
