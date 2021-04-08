using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.Permisstions
{
    public class PermissionRoleRelatePermission:BaseEntityGuid, IPermissionDescription
    {
        public virtual PermissionsRole Role { get; set; }
        public string RoleName { get; set; }
        /// <summary>
        /// 是否是自身权限
        /// </summary>
        public bool IsSelf { get; set; }
        /// <summary>
        /// 【冗余】权限名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 【冗余】权限作用域
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// 【冗余】权限类型
        /// </summary>
        public PermissionType Type { get; set; }
    }
}
