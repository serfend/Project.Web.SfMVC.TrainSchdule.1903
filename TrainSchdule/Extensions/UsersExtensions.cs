using System.Collections.Generic;
using Castle.Core.Internal;
using TrainSchdule.BLL.DTO;
using TrainSchdule.DAL.Entities;
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
                Avatar = GetAvatar(item),
                Date = item.Date.ToString(Info_DateFormat),
                Confirmed = item.Confirmed,
                Followed = item.Followed,
                Blocked = item.Blocked,
                PrivateAccount = item.PrivateAccount,
                IBlocked = item.IBlocked
            };
        }

        public const string ImgAvatarMale = "/images/defaults/def-male-logo.png";
        public const string ImgAvatarFemale = "/images/defaults/def-female-logo.png";
        public const string Info_DateFormat = "yyyy年MM月dd日";


        public static string GetAvatar(UserDTO item)
        {
	        if (!item.Avatar.IsNullOrEmpty()) return $"data/avatars/{item.UserName}/item.Avatar";
	        if ( item.Gender==GenderEnum.Male || item.Gender==GenderEnum.Unknown) return ImgAvatarMale;
	        return ImgAvatarFemale;
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
                    Avatar = GetAvatar(item),
                    Date = item.Date.ToString(Info_DateFormat),
                    Confirmed = item.Confirmed,
                    Followed = item.Followed,
                    Blocked = item.Blocked,
                    PrivateAccount = item.PrivateAccount,
                    IBlocked = item.IBlocked
                });
            }

            return users;
        }
    }
}