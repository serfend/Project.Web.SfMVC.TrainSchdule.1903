using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels;

namespace TrainSchdule.ViewModels.Verify
{
	public class ScrollerVerifyGeneratedViewModel:APIDataModel
	{
		public ScrollerVerifyGeneratedDataModel Data { get; set; }
	}

	public class ScrollerVerifyGeneratedDataModel
	{
		public string Id { get; set; }
		public int PosY { get; set; }
	}
}
