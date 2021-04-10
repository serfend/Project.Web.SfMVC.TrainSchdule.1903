using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.Permisstions
{
    public interface IPermissionDescription
    {

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
    public  class PermissionBaseItem: IPermissionDescription
    {

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 单位作用域
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// 权限操作类型
        /// </summary>
        public PermissionType Type { get; set; }
        /// <summary>
        /// 是否是自身角色的权限
        /// </summary>
        public bool IsSelf { get; set; }
    }
    public class PermissionGuidBaseItem : BaseEntityGuid ,IPermissionDescription
    {

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 单位作用域
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// 权限操作类型
        /// </summary>
        public PermissionType Type { get; set; }
    }
    /// <summary>
    /// 单个权限描述
    /// </summary>
    //public class PermissionItem: PermissionGuidBaseItem
    //{
    //}
}
