using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Apply
{
	public class SubmitBaseInfoViewModel
	{
		[Required]
		public string Id { get; set; }
		[Required]
		public string Company { get; set; }
		[Required]
		public string Duties { get; set; }
		[Required]
		public string HomeAddress { get; set; }
	}
}
