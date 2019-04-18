using System;
using TrainSchdule.DAL.Entities;

namespace TrainSchdule.BLL.DTO
{
    /// <summary>
    /// 用户信息的实体
    /// 包含 username, user real name, user avatar 等.
    /// </summary>
    public class UserDTO:BaseEntity
    {
        /// <summary>
        /// Gets and sets user real name.
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// Gets and sets username.
        /// </summary>
        public string UserName { get; set; }

		/// <summary>
		/// 授权码
		/// </summary>
		public string AuthKey { get; set; }

        /// <summary>
        /// Gets and sets user avatar.
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// Gets and sets user date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets and sets user gender.
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// Gets and sets user confirmed by amdmin state.
        /// </summary>
        public bool Confirmed { get; set; }

        /// <summary>
        /// Gets and sets user followed by current user state.
        /// </summary>
        public bool Followed { get; set; }

        /// <summary>
        /// Gets and sets user blocked by current user state.
        /// </summary>
        public bool Blocked { get; set; }

        /// <summary>
        /// Gets and sets user private account setting.
        /// </summary>
        public bool PrivateAccount { get; set; }

        /// <summary>
        /// Gets and sets user block current user state.
        /// </summary>
        public bool IBlocked { get; set; }
		/// <summary>
		/// 用户所在单位
		/// </summary>
        public CompanyDTO Company { get; set; }
		/// <summary>
		/// 用户权限，0为默认无权限
		/// </summary>
		public int Privilege { get; set; }
    }
}
