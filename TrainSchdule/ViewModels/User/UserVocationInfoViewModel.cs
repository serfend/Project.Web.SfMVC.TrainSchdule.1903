using BLL.Helpers;
using DAL.DTO.User;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	/// 用户休假信息
	/// </summary>
	public class UserVocationInfoViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public UserVocationInfoVDto Data { get; set; }
	}
}