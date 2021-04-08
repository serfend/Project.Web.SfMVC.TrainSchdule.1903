using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.Permisstions
{

    /// <summary>
    /// 角色所含权限
    /// </summary>
    public class PermissionItemRoleRelate : BaseEntityGuid
    {
        /// <summary>
        /// 所含权限
        /// </summary>
        public string  Permissions { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public virtual PermissionsRole Role { get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// 是否是角色自身权限
        /// </summary>
        public bool IsSelf { get; set; }
    }
}
