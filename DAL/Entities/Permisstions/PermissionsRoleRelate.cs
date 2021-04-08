using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.Permisstions
{

    /// <summary>
    /// 角色授权关系
    /// </summary>
    public class PermissionsRoleRelate:IHasGuidId
    {
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// 被授权的角色
        /// </summary>
        public virtual PermissionsRole To { get; set; }
        public string ToName { get; set; }
        /// <summary>
        /// 授权角色来源
        /// </summary>
        public virtual PermissionsRole From { get; set; }
        public string FromName { get; set; }
    }
}
