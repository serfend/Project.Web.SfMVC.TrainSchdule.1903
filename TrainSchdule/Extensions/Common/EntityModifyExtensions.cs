using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.Permisstions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Extensions.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class EntityModifyExtensions
    {
        /// <summary>
        /// 通用性 编辑实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">更新项</param>
        /// <param name="db">数据库</param>
        /// <param name="queryItemGetter">定义如何获取原项</param>
        /// <param name="companyGetter">定义如何获取用于授权的单位</param>
        /// <param name="auth">授权码</param>
        /// <param name="permission">授权权限</param>
        /// <param name="operation">操作</param>
        /// <param name="description">本次编辑描述</param>
        /// <param name="beforeModify"></param>
        /// <param name="beforeAdd"></param>
        /// <param name="googleAuthService">授权认证服务</param>
        /// <param name="usersService">用户服务</param>
        /// <param name="currentUserService">当前用户服务</param>
        /// <param name="userActionServices">用户行为记录服务</param>
        public static (ActionType, T) UpdateGuidEntity<T>(this T item, DbSet<T> db, Expression<Func<T, bool>> queryItemGetter, Func<T, string> companyGetter, GoogleAuthDataModel auth, Permission permission, PermissionType operation, string description, Action<T, T> beforeModify, Action<T> beforeAdd, IGoogleAuthService googleAuthService, IUsersService usersService, ICurrentUserService currentUserService, IUserActionServices userActionServices) where T : BaseEntityGuid
        {
            string permit_fail_company = null;
            var prevItem = db.Where(c => !c.IsRemoved).FirstOrDefault(queryItemGetter);
            if (auth == null) auth = new GoogleAuthDataModel();
            var authUser = auth.AuthUser(googleAuthService, usersService, currentUserService.CurrentUser);
            var prevCompany = prevItem == null ? null : companyGetter(prevItem);
            var company = companyGetter(item);
            ActionType action = ActionType.Remove;
            if (item.IsRemoved)
            {
                if (prevItem == null) throw new ActionStatusMessageException(item.NotExist());
                prevCompany = companyGetter(prevItem);
                var permit = userActionServices.Permission(authUser, permission, PermissionType.Write, prevCompany, $"{action}:{description}");
                if (permit)
                {
                    prevItem.Remove();
                    db.Update(prevItem);
                }
                else
                    permit_fail_company = prevCompany;
            }
            else
            {
                if (item == null) throw new ActionStatusMessageException(ActionStatusMessage.StaticMessage.IdIsNull);
                if (prevItem != null)
                {
                    action = ActionType.Update;
                    var permit = userActionServices.Permission(currentUserService.CurrentUser, permission, PermissionType.Write, prevCompany, $"通用更新原信息:{description}");
                    if (permit) permit = userActionServices.Permission(currentUserService.CurrentUser, permission, PermissionType.Write, company, $"通用更新新信息:{description}");
                    else permit_fail_company = prevCompany;
                    if (permit)
                    {
                        beforeModify?.Invoke(item, prevItem);
                        db.Update(prevItem);
                    }
                    else
                        permit_fail_company = company;
                }
                else
                {
                    action = ActionType.Add;
                    var permit = userActionServices.Permission(currentUserService.CurrentUser, permission, PermissionType.Write, company, $"通用新增:{description}");
                    if (permit)
                    {
                        beforeAdd?.Invoke(item);
                        db.Add(item);
                    }
                    else
                        permit_fail_company = company;
                }
            }

            if (permit_fail_company != null) throw new ActionStatusMessageException(new ApiResult(auth.PermitDenied(), $"需要授权[{permit_fail_company}]时被拒绝", true));
            return (action, item);
        }
        /// <summary>
        /// 
        /// </summary>
        public enum ActionType
        {
            /// <summary>
            /// 
            /// </summary>
            Add = 0,
            /// <summary>
            /// 
            /// </summary>
            Remove = 1,
            /// <summary>
            /// 
            /// </summary>
            Update = 2
        }
    }
}
