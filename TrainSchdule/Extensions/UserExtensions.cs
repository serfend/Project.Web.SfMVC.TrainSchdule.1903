using DAL.DTO.User;
using TrainSchdule.ViewModels.Account;

namespace TrainSchdule.Extensions
{
	/// <summary>
	/// 
	/// </summary>
	public static class UserExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <param name="invitedBy"></param>
		/// <returns></returns>
		public static UserCreateVdto ToDTO(this UserCreateDataModel model,string invitedBy)
		{
			if (model == null) return null;
			return new UserCreateVdto()
			{
				Company = model.Company,
				Duties = model.Duties,
				Email = model.Email,
				Gender = model.Gender,
				HomeAddress = model.HomeAddress,
				HomeDetailAddress = model.HomeDetailAddress,
				Id = model.Id,
				InvitedBy = invitedBy,
				Password = model.Password,
				RealName = model.RealName,
				Phone = model.Phone
			};
		}
	}
}
