using DAL.DTO.User;
using DAL.Entities.UserInfo;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Account
{
	public class UserCreateViewModel
	{
		public ScrollerVerifyViewModel Verify { get; set; }
		public GoogleAuthViewModel Auth { get; set; }
		public UserCreateDTO Data { get; set; }
	}
	

}
