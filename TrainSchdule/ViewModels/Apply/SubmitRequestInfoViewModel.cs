using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Apply
{
	public class SubmitRequestInfoViewModel
	{
		[Required]
		public string Id { get; set; }
		[Required]
		public DateTime?StampLeave { get; set; }
		[Required]
		public int VocationLength { get; set; }
		[Required]
		public int OnTripLength { get; set; }
		[Required]
		public string VocationType { get; set; }
		[Required]
		public int VocationPlace { get; set; }
		public string Reason { get; set; }
	}
}
