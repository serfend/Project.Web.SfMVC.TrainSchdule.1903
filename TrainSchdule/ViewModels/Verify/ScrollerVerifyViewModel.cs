using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;

namespace TrainSchdule.ViewModels.Verify
{
	public class ScrollerVerifyViewModel

	{
		private readonly IVerifyService _verifyService;

		public ScrollerVerifyViewModel(IVerifyService verifyService)
		{
			_verifyService = verifyService;
		}

		public bool Verify => _verifyService.Verify(Code);
		public int Code { get; set; }
	}
}
