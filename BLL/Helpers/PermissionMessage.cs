using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Helpers
{
    public static partial class ActionStatusMessage
    {
        public static class PermissionMessage
        {
            public static class Role
            {
                public static readonly ApiResult Exist = new ApiResult(100110, "角色已存在");
                public static readonly ApiResult NotExist = new ApiResult(100120, "角色不存在");

            }

            public static class Permission
            {
                public static readonly ApiResult Exist = new ApiResult(101110, "权限已存在");
                public static readonly ApiResult NotExist = new ApiResult(101120, "权限不存在");
            }
            public static class RoleRelateRole
            {
                public static readonly ApiResult Exist = new ApiResult(102110, "角色授权已存在");
                public static readonly ApiResult NotExist = new ApiResult(102120, "角色授权不存在");
                public static readonly ApiResult Deny = new ApiResult(102130, "角色授权被拒绝");
            }
            public static class RoleRelateUser
            {
                public static readonly ApiResult Exist = new ApiResult(103110, "用户角色已存在");
                public static readonly ApiResult NotExist = new ApiResult(103120, "用户角色不存在");
            }

            public static class RoleRelatePermission
            {
                public static readonly ApiResult Exist = new ApiResult(104110, "权限授权已存在");
                public static readonly ApiResult NotExist = new ApiResult(104120, "权限授权不存在");
                public static readonly ApiResult Deny = new ApiResult(104130, "权限授权被拒绝");
            }
        }
    }
}
