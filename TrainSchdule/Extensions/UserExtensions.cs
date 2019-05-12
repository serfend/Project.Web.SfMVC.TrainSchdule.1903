using System;
using DAL.DTO.User;
using DAL.Entities.UserInfo;
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
				Gender = model.Cid?.Length==18?Convert.ToInt32(model.Cid.Substring(16, 1))%2==1?GenderEnum.Male:GenderEnum.Female:GenderEnum.Unknown,
				HomeAddress = model.HomeAddress,
				HomeDetailAddress = model.HomeDetailAddress,
				Id = model.Id,
				Cid=model.Cid,
				InvitedBy = invitedBy,
				Password = model.Password,
				RealName = model.RealName,
				Phone = model.Phone,
				Settle = model.Settle
			};
		}
	}
}
