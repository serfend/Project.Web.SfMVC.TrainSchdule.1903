using DAL.Entities.Permisstions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public partial class ApplicationDbContext
    {
        /// <summary>
        /// 角色所含权限
        /// </summary>
        public DbSet<PermissionRoleRelatePermission> PermissionRoleRalatePermissions { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public DbSet<PermissionsRole> PermissionsRoles { get; set; }
        /// <summary>
        /// 角色所含角色
        /// </summary>
        public DbSet<PermissionsRoleRelate> PermissionsRoleRelates { get; set; }
        /// <summary>
        /// 用户所含角色
        /// </summary>
        public DbSet<PermissionsUserRelate> PermissionsUserRelates { get; set; }
        /// <summary>
        /// 【冗余】用户所含权限
        /// </summary>
        public DbSet<PermissionsUser> PermissionsUsers { get; set; }
            
    }
}
