using System;
using System.ComponentModel.DataAnnotations;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Apply
{
	public class SubmitApplyViewModel
	{
		[Required]
		public ScrollerVerifyViewModel Verify { get; set; }
		[Required]
		public Guid? BaseId { get; set; }
		[Required]
		public Guid? RequestId { get; set; }
	}

}
