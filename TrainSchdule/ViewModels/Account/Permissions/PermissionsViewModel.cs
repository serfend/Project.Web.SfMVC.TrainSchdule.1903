using DAL.Entities.Permisstions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Account.Permissions
{
    /// <summary>
    /// 
    /// </summary>
    public class PermissionRoleRelatePermissionViewModel
    {
        /// <summary>
        /// 角色名称
        /// </summary>
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
    /// <summary>
    /// 
    /// </summary>
    public class PermissionsRoleRelateViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 被授权的角色
        /// </summary>
        public string ToName { get; set; }
        /// <summary>
        /// 授权角色来源
        /// </summary>
        public string FromName { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class PermissionsExtensions {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static PermissionsRoleRelateViewModel ToModel(this PermissionsRoleRelate model) => new PermissionsRoleRelateViewModel() {
            Id = model.Id,
            FromName = model.FromName,
            ToName = model.ToName,
        };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static PermissionRoleRelatePermissionViewModel ToModel(this PermissionRoleRelatePermission model) => new PermissionRoleRelatePermissionViewModel() {
            IsSelf=model.IsSelf,
            Name=model.Name,
            Region=model.Region,
            RoleName=model.RoleName,
            Type=model.Type
        };

    }
}
