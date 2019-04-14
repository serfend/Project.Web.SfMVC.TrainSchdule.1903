using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.User
{
	public class UserProfileViewModel
	{
		[Required]
		[StringLength(32, ErrorMessage = "非法的{0}", MinimumLength = 2)]
		[Display(Name = "真实姓名")]
		public string RealName { get; set; }
		[Required]
		[StringLength(32, ErrorMessage = "非法的{0}", MinimumLength = 2)]
		[Display(Name = "单位路径")]
		public string Company { get; set; }
		[Required]
		[StringLength(32, ErrorMessage = "非法的{0}", MinimumLength = 2)]
		[Display(Name = "职务")]
		public string Duties { get; set; }
		[Required]
		[StringLength(32, ErrorMessage = "非法的{0}", MinimumLength = 2)]
		[Display(Name = "联系方式")]
		public string Phone { get; set; }
		[Required]
		[StringLength(32, ErrorMessage = "非法的{0}", MinimumLength = 2)]
		[Display(Name = "家庭住址")]
		public string Address { get; set; }

	}
}
