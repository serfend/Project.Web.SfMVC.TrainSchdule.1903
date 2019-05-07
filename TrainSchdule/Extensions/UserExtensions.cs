using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.DTO.User;
using TrainSchdule.ViewModels.Account;

namespace TrainSchdule.Extensions
{
	public static class UserExtensions
	{
		public static UserCreateVDTO ToDTO(this UserCreateDataModel model,string invitedBy)
		{
			if (model == null) return null;
			return new UserCreateVDTO()
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
