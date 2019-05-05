using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;

namespace TrainSchdule.ViewModels.Verify
{
	public class ScrollerVerifyViewModel
	{
		public bool Verify(IVerifyService _verifyService) => _verifyService.Verify(Code);
		public int Code { get; set; }
	}
}
