using System;
using System.Collections.Generic;
using DAL.Entities;

namespace TrainSchdule.DAL.Entities
{
    /// <summary>
    /// User entity (not identity).
    /// Contains user properties.
    /// <see cref="UserName"/> is key value equal to <see cref="UserName"/> in <see cref="ApplicationUser"/>.
    /// </summary>
    public class User : BaseEntity
    {
        #region Properties
        /// <summary>
        /// 用户权限，0为默认无权限
        /// </summary>
        public int Privilege { get; set; }

        /// <summary>
        /// Gets and sets user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets and sets user real name.
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// Gets and sets user avatar.
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// Gets and sets user about section.
        /// </summary>
        public string About { get; set; }

        /// <summary>
        /// Gets and sets user date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets and sets user website link.
        /// </summary>
        public string WebSite { get; set; }

        /// <summary>
        /// Gets and sets user gender.
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// Gets and sets user private account config.
        /// </summary>
        public bool PrivateAccount { get; set; }
		/// <summary>
		/// 用户所处的单位
		/// </summary>
        public virtual Company Company { get; set; }

		public string Address { get; set; }

		public virtual Duties Duties { get; set; }

		public string Phone { get; set; }
		/// <summary>
		/// 用户可操作的单位
		/// </summary>
		public virtual IEnumerable<PermissionCompany> PermissionCompanies { get; set; }
        #endregion

        #region .ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/>, sets property to default values.
        /// </summary>
        public User()
        {
            PrivateAccount = false;
            Date = DateTime.Now;
        }

        #endregion
    }
}
