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
        public static UserDetailsDTO ToDTO(this User item)
        {
            if (item == null)
            {
                return null;
            }

            var permissionCompanies = new List<PermissionCompanyDTO>();
            foreach (var itemPermissionCompany in item.PermissionCompanies)
            {
	            permissionCompanies.Add(itemPermissionCompany.ToDTO());
            }

			return new UserDetailsDTO
            {
                RealName = item.RealName,
                UserName = item.UserName,
                Avatar = item.Avatar,
                About = item.About,
                Date = item.Date,
                Gender = (GenderEnum)item.Gender,
                WebSite = item.WebSite,
                PrivateAccount = item.PrivateAccount,
				PermissionCompanies = permissionCompanies,
				Address = item.Address,
				Phone = item.Phone,
				Company = item.Company.ToDTO()

            };
        }

        /// <summary>
        /// Maps user entity to user details DTO.
        /// </summary>
        public static UserDetailsDTO ToDTO(this User item, bool confirmed, bool followed, bool blocked, bool iBlocked, ICollection<UserDTO> followings, ICollection<UserDTO> followers, ICollection<UserDTO> mutuals)
        {
            if (item == null)
            {
                return null;
            }

            var result = ToDTO(item);
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
