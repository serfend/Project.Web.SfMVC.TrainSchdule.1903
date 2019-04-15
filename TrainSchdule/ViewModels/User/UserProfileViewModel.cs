using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.User
{
	public class UserProfileViewModel
	{
		[StringLength(32, ErrorMessage = "非法的{0}", MinimumLength = 2)]
		public string RealName { get; set; }
		[StringLength(64, ErrorMessage = "非法的{0}", MinimumLength = 2)]
		public string Company { get; set; }
		[StringLength(32, ErrorMessage = "非法的{0}", MinimumLength = 2)]
		public string Duties { get; set; }
		[StringLength(16, ErrorMessage = "非法的{0}", MinimumLength = 2)]
		public string Phone { get; set; }
		[StringLength(64, ErrorMessage = "非法的{0}", MinimumLength = 2)]
		public string Address { get; set; }

	}
}
