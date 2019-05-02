using System.Collections.Generic;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.BLL.DTO.UserInfo;
using TrainSchdule.DAL.Entities.UserInfo;
using TrainSchdule.ViewModels.Company;
using TrainSchdule.WEB.ViewModels;

namespace TrainSchdule.WEB.Extensions
{
    /// <summary>
    /// Methods for mapping user DTOs to user view models.
    /// </summary>
    public static class UsersExtensions
    {
        /// <summary>
        /// Maps user DTO to user view model.
        /// </summary>
        public static UserViewModel ToViewModel(this UserDTO item)
        {
            if (item == null)
            {
                return null;
            }

            return new UserViewModel
            {
                RealName = item.RealName,
                UserName = item.UserName,
                Avatar = item.Avatar,
                Date = item.Date.ToString(BLL.Extensions.UsersExtensions.Info_DateFormat),
                Confirmed = item.Confirmed,
                Followed = item.Followed,
                Blocked = item.Blocked,
                PrivateAccount = item.PrivateAccount,
                IBlocked = item.IBlocked
            };
        }

        /// <summary>
        /// Maps user DTOs to user view models.
        /// </summary>
        public static List<UserViewModel> ToViewModels(this IEnumerable<UserDTO> items)
        {
            if (items == null)
            {
                return null;
            }

            var users = new List<UserViewModel>();

            foreach (var item in items)
            {
                users.Add(new UserViewModel
                {
                    RealName = item.RealName,
                    UserName = item.UserName,
                    Avatar = item.Avatar,
                    Date = item.Date.ToString(BLL.Extensions.UsersExtensions.Info_DateFormat),
                    Confirmed = item.Confirmed,
                    Followed = item.Followed,
                    Blocked = item.Blocked,
                    PrivateAccount = item.PrivateAccount,
                    IBlocked = item.IBlocked
                });
            }

            return users;
        }

        public static CompanySingleMemberDataModel ToCompanyMembersDataModel(this User user)
        {
			var c=new CompanySingleMemberDataModel()
			{
				Company = user.Company.Name,
				Duties = user.Duties?.Name,
				RealName = user.RealName??user.UserName,
				UserName = user.UserName,
				Gender = user.Gender
			};
			return c;
        }
    }
}