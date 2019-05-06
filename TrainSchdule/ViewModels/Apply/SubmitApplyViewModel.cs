using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Apply
{
	public class SubmitApplyViewModel
	{
		public ScrollerVerifyViewModel Verify { get; set; }
		public Guid BaseId { get; set; }
		public Guid RequestId { get; set; }
	}
}
