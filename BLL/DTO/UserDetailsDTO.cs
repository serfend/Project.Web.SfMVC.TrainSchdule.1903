using System.Collections.Generic;
using DAL.Entities;

namespace TrainSchdule.BLL.DTO
{
    /// <summary>
    /// User details data transfer object.
    /// Inherits properties from user data transfer object, also contains user about info, user website, mutuals collection, followings collection, followers collection.
    /// </summary>
    public class UserDetailsDTO : UserDTO
    {
	    /// <summary>
		/// 用户修改单位的权限
		/// </summary>
	    public IEnumerable<PermissionCompanyDTO> PermissionCompanies { get; set; }
		public string Address { get; set; }
	    public string Phone { get; set; }
	    /// <summary>
	    /// 用户职务
	    /// </summary>
	    public Duties Duties { set; get; }
    
        /// <summary>
        /// Gets and sets user about.
        /// </summary>
        public string About { get; set; }

        /// <summary>
        /// Gets and sets user website.
        /// </summary>
        public string WebSite { get; set; }

        /// <summary>
        /// Gets and sets mutuals collection of mutual user DTOs.
        /// </summary>
        public IEnumerable<UserDTO> Mutuals { get; set; }

        /// <summary>
        /// Gets and sets followings collection of user following DTOs.
        /// </summary>
        public IEnumerable<UserDTO> Followings { get; set; }

        /// <summary>
        /// Gets and sets followers collection of user follower DTOs.
        /// </summary>
        public IEnumerable<UserDTO> Followers { get; set; }
    }
}
