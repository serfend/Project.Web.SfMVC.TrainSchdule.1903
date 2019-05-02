using Castle.Core.Internal;
using TrainSchdule.DAL.Entities.UserInfo;
using TrainSchdule.BLL.DTO.UserInfo;
using TrainSchdule.BLL.Helpers;
using TrainSchdule.DAL.Entities.UserInfo.Permission;
using User = TrainSchdule.DAL.Entities.UserInfo.User;

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
				Id = item.Id,
				Privilege = item.Privilege,
                RealName = item.RealName,
                UserName = item.UserName,
				AuthKey = item.AuthKey,
                Avatar = GetAvatar(item),
                Date = item.Date,
                Gender = (GenderEnum)item.Gender,

                Confirmed = false,
                Followed = false,
                Blocked = false,
                PrivateAccount = item.PrivateAccount,
                IBlocked = false
            };
        }

        public const string ImgAvatarMale = "/images/defaults/def-male-logo.png";
        public const string ImgAvatarFemale = "/images/defaults/def-female-logo.png";
        public const string Info_DateFormat = "yyyy年MM月dd日";


        public static string GetAvatar(User item)
        {
	        if (!item.Avatar.IsNullOrEmpty()) return $"data/avatars/{item.UserName}/item.Avatar";
	        if (item.Gender == GenderEnum.Male || item.Gender == GenderEnum.Unknown) return ImgAvatarMale;
	        return ImgAvatarFemale;
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

            var user = ToDTO(item);
            user.Confirmed = confirmed;
            user.Followed = followed;
            user.Blocked = blocked;
            user.IBlocked = iBlocked;
            return user;
        }
    }
}
