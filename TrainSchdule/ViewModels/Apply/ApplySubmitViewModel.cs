using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using TrainSchdule.ViewModels.Static;

namespace TrainSchdule.ViewModels.Apply
{
	public class ApplySubmitViewModel:VerifyViewModel
	{
		public ApplySubmitData Param { get; set; }
	}

	public class ApplySubmitData
	{
		[DisplayName("request")]
		public ApplyRequest Request { get; set; }

		public string xjlb { get; set; }
		[DisplayName("stamp")]
		public ApplyStamp Stamp { get; set; }
		[DisplayName("to")]
		public IEnumerable<int> To { get; set; }
	}
}
