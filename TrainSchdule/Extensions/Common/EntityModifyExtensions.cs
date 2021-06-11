using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.Permisstions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// 编辑实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="db"></param>
        /// <param name="queryItemGetter"></param>
        /// <param name="companyGetter"></param>
        /// <param name="auth"></param>
        /// <param name="permission"></param>
        /// <param name="operation"></param>
        /// <param name="description"></param>
        /// <param name="googleAuthService"></param>
        /// <param name="usersService"></param>
        /// <param name="currentUserService"></param>
        /// <param name="userActionServices"></param>
        public static ActionType UpdateGuidEntity<T>(this T item, DbSet<T> db, Func<T, bool> queryItemGetter, Func<T, string> companyGetter, GoogleAuthDataModel auth, Permission permission, PermissionType operation, string description, IGoogleAuthService googleAuthService, IUsersService usersService, ICurrentUserService currentUserService, IUserActionServices userActionServices) where T : BaseEntity
        {
            string permit_fail_company = null;
            var prevItem = db.Where(c=>!c.IsRemoved).FirstOrDefault(queryItemGetter);
            var authUser = auth.AuthUser(googleAuthService, usersService, currentUserService.CurrentUser);
            var prevCompany = prevItem == null ? null : companyGetter(prevItem);
            var company = companyGetter(item);
            ActionType action = ActionType.Remove;
            if (item.IsRemoved)
            {
                if (prevItem == null) throw new ActionStatusMessageException(item.NotExist());
                prevCompany = companyGetter(prevItem);
                permit_fail_company = userActionServices.Permission(authUser, ApplicationPermissions.Client.Manage.Info.Item, PermissionType.Write, prevCompany, $"{action}:{description}") ? null : prevCompany;
                if (permit_fail_company == null)
                {
                    prevItem.Remove();
                    db.Update(prevItem);
                }
            }
            else
            {
                if (item == null) throw new ActionStatusMessageException(ActionStatusMessage.StaticMessage.IdIsNull);
                if (prevItem != null)
                {
                    action = ActionType.Update;
                    permit_fail_company = userActionServices.Permission(currentUserService.CurrentUser, ApplicationPermissions.Client.Manage.Info.Item, PermissionType.Write, prevCompany, $"更新原信息:{description}") ? null : prevCompany;
                    if (permit_fail_company == null) permit_fail_company = userActionServices.Permission(currentUserService.CurrentUser, ApplicationPermissions.Client.Manage.Info.Item, PermissionType.Write, company, $"更新新信息:{description}") ? null : company;
                    if (permit_fail_company == null)
                    {
                        db.Remove(prevItem);
                        db.Add(item);
                    }
                }
                else
                {
                    action = ActionType.Add;
                    permit_fail_company = userActionServices.Permission(currentUserService.CurrentUser, ApplicationPermissions.Client.Manage.Info.Item, PermissionType.Write, company, "新增标签内容") ? null : company;
                    if (permit_fail_company == null) db.Add(item);
                }
            }

            if (permit_fail_company != null) throw new ActionStatusMessageException(new ApiResult(auth.PermitDenied(), $"授权到{permit_fail_company}", true));
            return action;
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
