using BLL.Interfaces;

namespace TrainSchdule.ViewModels.Verify
{
	public class ScrollerVerifyViewModel
	{
		public bool Verify(IVerifyService _verifyService) => _verifyService.Verify(Code);
		public int Code { get; set; }
	}
}
