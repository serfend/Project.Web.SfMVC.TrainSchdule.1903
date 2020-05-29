using BLL.Helpers;
using DAL.DTO.User;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	/// 用户休假信息
	/// </summary>
	public class UserVacationInfoViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public UserVacationInfoVDto Data { get; set; }
	}
}