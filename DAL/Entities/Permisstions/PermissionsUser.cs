using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.Permisstions
{



    /// <summary>
    /// 用户权限
    /// </summary>
    public class PermissionsUser : IPermissionDescription,IHasGuidId
    {
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public virtual User User { get; set; }
        public string UserId { get; set; }

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
    /// <summary>
    /// 权限操作类型
    /// </summary>
    [Flags]
    public enum PermissionType
    {
        None = 0,
        Read = 1,
        Write = 2,
        BanRead = 4,
        BanWrite = 8
    }
}
