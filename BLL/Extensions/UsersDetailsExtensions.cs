using System.Collections.Generic;
using TrainSchdule.DAL.Entities;
using TrainSchdule.BLL.DTO;

namespace TrainSchdule.BLL.Extensions
{
    /// <summary>
    /// Methods for mapping user entities to user data transfer objects.
    /// </summary>
    public static class UsersDetailsExtensions
    {
        /// <summary>
        /// Maps user entity to user details DTO.
        /// </summary>
        public static UserDetailsDTO ToDetailDTO(this User item)
        {
            if (item == null)
            {
                return null;
            }



			var tmp= new UserDetailsDTO
            {
				Id = item.Id,
				AuthKey = item.AuthKey,
				Duties = item.Duties,
				Privilege = item.Privilege,
                RealName = item.RealName,
                UserName = item.UserName,
				InvitedBy = item.InvitedBy,
                Avatar =UsersExtensions.GetAvatar(item) ,
                About = item.About,
                Date = item.Date,
                Gender = (GenderEnum)item.Gender,
                WebSite = item.WebSite,
                PrivateAccount = item.PrivateAccount,
				Permissions = item.Permission,
				Address = item.Address,
				Phone = item.Phone,


			};
			var tmpCmp = item.Company;
			tmp.Company = tmpCmp.ToDTO();
			return tmp;

        }

        /// <summary>
        /// Maps user entity to user details DTO.
        /// </summary>
        public static UserDetailsDTO ToDetailDTO(this User item, bool confirmed, bool followed, bool blocked, bool iBlocked, ICollection<UserDTO> followings, ICollection<UserDTO> followers, ICollection<UserDTO> mutuals)
        {
            if (item == null)
            {
                return null;
            }

            var result = ToDetailDTO(item);
            result.Confirmed = confirmed;
            result.Followed = followed;
            result.Blocked = blocked;
            result.IBlocked = iBlocked;
            result.Followings = followings;
            result.Followers = followers;
            result.Mutuals = mutuals;
            return result;
        }
    }
}
