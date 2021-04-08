using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.Permisstions
{
    /// <summary>
    /// 用户所含角色
    /// </summary>
    public class PermissionsUserRelate : BaseEntityGuid
    {
        /// <summary>
        /// 用户
        /// </summary>
        public virtual User User { get; set; }
        public string UserId { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public virtual PermissionsRole Role { get; set; }
        public string RoleName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Create { get; set; }
    }

}
