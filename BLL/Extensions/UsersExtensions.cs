﻿using TrainSchdule.DAL.Entities;
using TrainSchdule.BLL.DTO;

namespace TrainSchdule.BLL.Extensions
{
    /// <summary>
    /// Methods for mapping user entities to user data transfer objects.
    /// </summary>
    public static class UsersExtensions
    {
        /// <summary>
        /// Maps user entity to user DTO.
        /// </summary>
        public static UserDTO ToDTO(this User item)
        {
            if (item == null)
            {
                return null;
            }

            return new UserDTO
            {
				Privilege = item.Privilege,
                RealName = item.RealName,
                UserName = item.UserName,
                Avatar = item.Avatar,
                Date = item.Date,
                Gender = (GenderEnum)item.Gender,

                Confirmed = false,
                Followed = false,
                Blocked = false,
                PrivateAccount = item.PrivateAccount,
                IBlocked = false
            };
        }

        /// <summary>
        /// Maps user entity to user DTO.
        /// </summary>
        public static UserDTO ToDTO(this User item, bool confirmed, bool followed, bool blocked, bool iBlocked)
        {
            if (item == null)
            {
                return null;
            }

            return new UserDTO
            {
                RealName = item.RealName,
                UserName = item.UserName,
                Avatar = item.Avatar,
                Date = item.Date,
                Gender = (GenderEnum)item.Gender,

                Confirmed = confirmed,
                Followed = followed,
                Blocked = blocked,
                PrivateAccount = item.PrivateAccount,
                IBlocked = iBlocked
            };
        }
    }
}
