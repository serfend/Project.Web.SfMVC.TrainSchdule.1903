using System;
using DAL.DTO.User;
using DAL.Entities;
using DAL.Entities.UserInfo;
using Microsoft.EntityFrameworkCore;
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
		/// <param name="db"></param>
		/// <returns></returns>
		public static UserCreateVdto ToDTO(this UserCreateDataModel model,string invitedBy,DbSet<AdminDivision> db)
		{
			if (model == null) return null;
			return new UserCreateVdto()
			{
				Company = model.Company,
				Duties = model.Duties,
				Email = model.Email,
				Gender = model.Cid?.Length==18?Convert.ToInt32(model.Cid.Substring(16, 1))%2==1?GenderEnum.Male:GenderEnum.Female:GenderEnum.Unknown,
				Id = model.Id,
				Cid=model.Cid,
				InvitedBy = invitedBy,
				Password = model.Password,
				RealName = model.RealName,
				Phone = model.Phone,
				Settle = new DAL.Entities.UserInfo.Settle.Settle()
				{
					Self=model.Settle.Self.ToMoment(db),
					Lover=model.Settle.Lover.ToMoment(db),
					Parent=model.Settle.Parent.ToMoment(db)
				}
			};
		}
	}
}
