using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Abp.Extensions;
using BLL.Extensions;
using BLL.Extensions.Common;
using BLL.Extensions.PermissionServicesExtension;
using BLL.Helpers;
using DAL.Entities.Permisstions;
using DAL.Entities.UserInfo;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Permission
{
    public partial class PermissionServices
    {
        public DAL.Entities.Permisstions.Permission GetPermissionByName(string name) => PermissionDictionaryExtensions.DictPermissions.ContainsKey(name) ? PermissionDictionaryExtensions.DictPermissions[name] : null;
        public (PermissionsRole, IEnumerable<string>, IEnumerable<string>, IEnumerable<PermissionBaseItem>) RoleDetail(string role)
        {
            var r = context.PermissionsRoles.FirstOrDefault(i=>i.Name==role)?? throw new ActionStatusMessageException(new PermissionsRole().NotExist());
            // 本权限得到的授权
            var roleFromRelate = context.PermissionsRoleRelates.Where(i => i.ToName == role).Select(i => i.FromName).Distinct().ToList();
            // 本权限发起的授权
            var roleToRelate = context.PermissionsRoleRelates.Where(i => i.FromName == role).Select(i => i.ToName).Distinct().ToList();
            var rolePermission = context.PermissionRoleRalatePermissions.Where(i => i.RoleName == role).Select(i => new PermissionBaseItem()
            {
                Name = i.Name,
                Region = i.Region,
                Type = i.Type,
                IsSelf = i.IsSelf
            }).ToList();
            return (r,roleFromRelate,roleToRelate,rolePermission);
        }
        public PermissionsRole RoleModify(string role,string createBy,bool isRemove)
        {
            var db = context.PermissionsRoles;
            var r = db.FirstOrDefault(i => i.Name == role);
            if (r != null && !isRemove) return r;
            if(r==null && isRemove) return null;
            if (isRemove)
            {
                // 移除角色关联的角色
                void RemoveRoleRelateRole(string name)
                {
                    var dbRelateRole = context.PermissionsRoleRelates;
                    var relateRoles = dbRelateRole.Where(i => i.FromName == name || i.ToName == name);
                    dbRelateRole.RemoveRange(relateRoles);
                }
                // 移除角色关联的权限
                void RemoveRoleRelatePermissions(string name)
                {
                    var dbRelatePermission = context.PermissionRoleRalatePermissions;
                    var relatePermissions = dbRelatePermission.Where(i => i.RoleName == name);
                    dbRelatePermission.RemoveRange(relatePermissions);
                }
                // 移除用户关联的角色
                void RemoveUserRelateRole(string name) {
                    var dbUserRoles = context.PermissionsUserRelates;
                    var relateRoles = dbUserRoles.Where(r => r.RoleName == name);
                    var shallUpdateUsers = relateRoles.Select(i => i.UserId).Distinct();
                    foreach(var u in shallUpdateUsers)
                        BackgroundJob.Enqueue<PermissionServices>(s => s.SyncUserPermissions(u,10));
                    dbUserRoles.RemoveRange(relateRoles);
                    
                }
                RemoveRoleRelatePermissions(role);
                RemoveRoleRelateRole(role);
                RemoveUserRelateRole(role);
                db.Remove(r);
                context.SaveChanges();
            }
            else
            {
                r = new PermissionsRole() {
                    Create = DateTime.Now,
                    CreateById = createBy,
                    Name = role
                };
                db.Add(r);
                context.SaveChanges();
            }
            return r;
        }
        public PermissionRoleRelatePermission RoleRelatePermissions(string role, IPermissionDescription permission)
        {
            var db = context.PermissionRoleRalatePermissions;
            var role_item = context.PermissionsRoles.FirstOrDefault(i => i.Name == role) ?? throw new ActionStatusMessageException(new PermissionsRole().NotExist());
            // 获取本权限关联本角色情况
            var current = db.Where(i => i.IsSelf).Where(i => i.RoleName == role).Where(i => i.Name == permission.Name).Where(i => i.Region == permission.Region).FirstOrDefault();
            if (current == null)
            {
                // 新增角色关联
                current = new PermissionRoleRelatePermission() { RoleName = role,IsSelf=true };
                MapIPermissionDescription(current, permission);
                if (current.Type!=PermissionType.None) db.Add(current);
                else throw new ActionStatusMessageException(current.NotExist());
            }
            else
            {
                if (permission.Type == PermissionType.None)
                    db.Remove(current);
                else
                {
                    if(current.Type == permission.Type) // 判断权限动作是否有变化
                        throw new ActionStatusMessageException(current.Exist());
                    current.Type = permission.Type; // 冗余记录
                    db.Update(current);
                }
            }
            context.SaveChanges();
            BackgroundJob.Enqueue<PermissionServices>(s=>s.SyncRolePermissions(role,10));
            return current;
        }
        /// <summary>
        /// 匹配字段
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private static void MapIPermissionDescription(IPermissionDescription a, IPermissionDescription b)
        {
            a.Name = b.Name;
            a.Type = b.Type;
            a.Region = b.Region;
        }
        public PermissionsRoleRelate RoleRelateRole(PermissionsRole from, PermissionsRole to, bool isRemove)
        {
            var r = context.PermissionsRoleRelates.Where(r => r.FromName == from.Name).Where(r => r.ToName == to.Name).FirstOrDefault();
            if (r != null && !isRemove) throw new ActionStatusMessageException(r.Exist());
            if (r == null && isRemove) throw new ActionStatusMessageException(r.NotExist());

            if (isRemove)
                context.PermissionsRoleRelates.Remove(r);
            else
            {
                r = new PermissionsRoleRelate() { 
                    From =from,
                    To=to,
                };
                context.PermissionsRoleRelates.Add(r);
            }
            context.SaveChanges();
            BackgroundJob.Enqueue<PermissionServices>(s => s.SyncRolePermissions(to.Name,10));
            return r;
        }
        public PermissionsRoleRelate RoleRelateRole(string from, string to, bool isRemove)
        {
            var from_item = context.PermissionsRoles.FirstOrDefault(r => r.Name == from);
            if (from_item == null) throw new ActionStatusMessageException(from_item.NotExist());
            var to_item = context.PermissionsRoles.FirstOrDefault(r => r.Name == to);
            if (to_item == null) throw new ActionStatusMessageException(to_item.NotExist());
            return RoleRelateRole(from_item, to_item, isRemove);
        }

        public PermissionsRoleRelate RoleRelateRole(string role, User userAuthBy, string userAuthTo)
        {
            var dbRoles = context.PermissionsRoles;
            var roleItem = dbRoles.FirstOrDefault(i => i.Name == role);
            if(roleItem==null) throw new ActionStatusMessageException(ActionStatusMessage.PermissionMessage.Role.NotExist);
            var userRoles = context.PermissionsUserRelates.Where(r => r.UserId == userAuthBy.Id);
            var userRole = userRoles.FirstOrDefault(r => r.RoleName == role);
            if (userRole == null)
            {
                // 自身已有的角色可直接派生授权，自身无权限的以主管方式派生授权
                var permissions = context.PermissionRoleRalatePermissions.Where(r => r.RoleName == role).Select(p => p.Region).Distinct();
                var permit = permissions.ToList().All(p =>
                {
                    var checkCompanyMajor = userAuthBy.CheckCompanyMajor(p);
                    if (checkCompanyMajor)
                    {
                        return true;
                    }
                    var checkCompanyManager = userAuthBy.CheckCompanyManager(p, userServiceDetail);
                    if (checkCompanyManager)
                    {
                        return true;
                    }
                    throw new ActionStatusMessageException(new ApiResult(ActionStatusMessage.PermissionMessage.RoleRelateRole.Deny, $"授权到{p}失败", true));
                });
            }
                
            // 新增新的角色
            var r = RoleModify($"{role}@{userAuthTo}",userAuthBy.Id, false);
            //if (dbRoles.FirstOrDefault(ra => ra.Name == r.Name) != null) throw new ActionStatusMessageException(ActionStatusMessage.PermissionMessage.RoleRelateRole.Exist);
            //dbRoles.Add(r);

           
            var newRelate = context.PermissionsRoleRelates.FirstOrDefault(i => i.ToName == r.Name && i.FromName == roleItem.Name);
            if (newRelate == null)
            {
                newRelate = new PermissionsRoleRelate()
                {
                    From = roleItem,
                    To = r,
                };
                context.PermissionsRoleRelates.Add(newRelate);
            }
            context.SaveChanges();
            BackgroundJob.Enqueue<PermissionServices>(s => s.SyncRolePermissions(r.Name,10));
            BackgroundJob.Enqueue<PermissionServices>(s => s.UserRalteRole(userAuthTo, r.Name, false));
            return newRelate;
        }
        public void SyncRolePermissions(string role,int maxTraceTime)
        {
            // 清除所有关联角色带来的权限
            var currentList = context.PermissionRoleRalatePermissions.Where(i => i.RoleName == role).Where(i=>!i.IsSelf);
            context.PermissionRoleRalatePermissions.RemoveRange(currentList);
            // 重建关联角色带来的权限
            var roleByOthersRoleName = context.PermissionsRoleRelates.Where(i => i.ToName == role).Select(r=>r.FromName).Distinct().ToList();
            var roleByOthers = context.PermissionRoleRalatePermissions.Where(p => roleByOthersRoleName.Contains(p.RoleName));
            var list = new List<IPermissionDescription>();
            foreach (var p in roleByOthers)
                list.Add(p);
            list = list.ToList().DistinctByPermission();
            foreach (var p in list)
            {
                var permission = new PermissionRoleRelatePermission()
                {
                    RoleName = role
                };
                MapIPermissionDescription(permission, p);
                context.PermissionRoleRalatePermissions.Add(permission);
            }
            context.SaveChanges();
            // 重建其他依赖本角色的角色
            var roleToOthersRoleName = context.PermissionsRoleRelates.Where(i => i.FromName == role).Select(r => r.ToName).Distinct().ToList();
            foreach (var r in roleToOthersRoleName)
                BackgroundJob.Enqueue<PermissionServices>(s => s.SyncRolePermissions(r, maxTraceTime - 1));
            BackgroundJob.Enqueue<PermissionServices>(s=>s.SyncUserPermissionsOfRole(role));
        }

        public PermissionsUserRelate UserRalteRole(string user, string role, bool isRemove)
        {
            var current = context.PermissionsUserRelates.Where(i => i.RoleName == role).Where(i => i.UserId == user).FirstOrDefault();
            if(current!=null && !isRemove) throw new ActionStatusMessageException(current.Exist());
            if(current==null && isRemove) throw new ActionStatusMessageException(current.NotExist());
            if (isRemove)
                context.PermissionsUserRelates.Remove(current);
            else {
                var dbRoles = context.PermissionsRoles;
                var r = dbRoles.FirstOrDefault(i => i.Name == role);
                if(r==null) throw new ActionStatusMessageException(r.NotExist());
                current = new PermissionsUserRelate()
                {
                    Create = DateTime.Now,
                    RoleName = role,
                    UserId = user
                };
                context.PermissionsUserRelates.Add(current);
            }
            context.SaveChanges();
            BackgroundJob.Enqueue<PermissionServices>(s=>s.SyncUserPermissions(user,10));
            return current;
        }
        /// <summary>
        /// 同步角色关联的所有用户权限
        /// </summary>
        /// <param name="name"></param>
        public void SyncUserPermissionsOfRole(string name)
        {
            var affectUsers = context.PermissionsUserRelates.Where(u => u.RoleName == name).Select(u => u.User).Select(u=>u.Id).Distinct().ToList();
            // 同步所有用户的所有权限以修复前期可能的错误
            foreach (var u in affectUsers)
                BackgroundJob.Enqueue<PermissionServices>(s => s.SyncUserPermissions(u, 10));
        }
        public void SyncUserPermissions(string user, int maxTraceTime)
        {
            // 移除原有权限记录
            void RemoveOrginPermissions()
            {
                var prevList = context.PermissionsUsers.Where(u => u.UserId == user);
                context.PermissionsUsers.RemoveRange(prevList);
            }
            // 用户所含角色
            List<IPermissionDescription> CheckUserPermissions()
            {
                var userRolesName = context.PermissionsUserRelates.Where(u => u.UserId == user).Select(r => r.RoleName).Distinct().ToList();
                var userRoles = context.PermissionRoleRalatePermissions.Where(p => userRolesName.Contains(p.RoleName)).ToList();
                var list = new List<IPermissionDescription>();
                foreach (var p in userRoles)
                    list.Add(p);
                return list;
            }
            // 角色应有权限
            void AttachUserPermissions(List<IPermissionDescription> list)
            {
                list = list.ToList().DistinctByPermission();
                foreach (var p in list)
                {
                    var permission = new PermissionsUser()
                    {
                        UserId = user
                    };
                    MapIPermissionDescription(permission, p);
                    context.PermissionsUsers.Add(permission);
                }
            }

            RemoveOrginPermissions();
            var list = CheckUserPermissions();
            AttachUserPermissions(list);
            context.SaveChanges();
        }
        public IEnumerable<string> RolePermissionCompany(string role)
        {
            var dbRelatePermissions = context.PermissionRoleRalatePermissions;
            var currentList = dbRelatePermissions.Where(i => i.RoleName == role).Select(i=>i.Region).Distinct().ToList();
            var result = new List<string>();
            foreach(var c in currentList)
            {
                if (c.IsNullOrEmpty()) continue;
                bool has_higher = false; // 是否已有更高权限项
                for(var i = 0; i < result.Count; i++)
                {
                    if (c.StartsWith(result[i]))
                    {
                        result.RemoveAt(i);
                        has_higher = true;
                        break;
                    }
                }
                if (!has_higher) result.Add(c);
            }
            return result;
        }
    }
    public class IPermissionNode
    {
        public IPermissionDescription Item;
        public string Key;
    }

    public static class PermissionExtensions
    {

        public static List<IPermissionDescription> DistinctByPermission(this List<IPermissionDescription> list)
        {
            var r = new List<IPermissionNode>();
            list.ToList().ForEach(v =>
            {
                var key = v.Name.Split('.')
                    .Reverse()
                    .Concat(new List<string>() { "#" })
                    .Concat(
                        v.Region.ToCharArray()
                            .Select(i => i.ToString()))
                    .ToList();
                r.Add(new IPermissionNode() { Key = string.Join('.', key), Item = v });
            });
            return r.DistinctByCompany(t => t.Key, (a, b) => a.Contains(b)).Select(i => i.Item).ToList();
        }
    }
}
