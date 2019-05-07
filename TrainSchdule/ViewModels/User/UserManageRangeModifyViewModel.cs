using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.User
{
	public class UserManageRangeModifyViewModel
	{
		[Required]
		public GoogleAuthViewModel Auth { get; set; }
		[Required]
		public string Code { get; set; }
		public string Id { get; set; }
	}
}
