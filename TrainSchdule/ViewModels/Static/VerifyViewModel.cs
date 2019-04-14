using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Static
{
	public class VerifyViewModel
	{
		[Display(Name = "验证码")]
		public int Verify { get; set; }
	}
}
