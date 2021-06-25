using Abp.Extensions;
using BLL.Extensions.Common;
using BLL.Extensions.PermissionServicesExtension;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.Permission;
using BLL.Services.Permission;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.Permisstions;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using TrainSchdule.ViewModels.Account;
using TrainSchdule.ViewModels.Account.Permissions;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers
{
    /// <summary>
    /// 权限管理
    /// </summary>
    [Authorize]
    [Route("[controller]/[action]")]
    public partial class PermissionController : Controller
    {
        private readonly IPermissionServices permissionServices;
        private readonly ICurrentUserService currentUserService;
        private readonly IUsersService usersService;
        private readonly ApplicationDbContext context;
        private readonly IUserActionServices userActionServices;
        private readonly IGoogleAuthService googleAuthService;

        /// <summary>
        /// 
        /// </summary>
        public PermissionController(IPermissionServices permissionServices, ICurrentUserService currentUserService, IUsersService usersService, ApplicationDbContext context, IUserActionServices userActionServices, IGoogleAuthService googleAuthService)
        {
            this.permissionServices = permissionServices;
            this.currentUserService = currentUserService;
            this.usersService = usersService;
            this.context = context;
            this.userActionServices = userActionServices;
            this.googleAuthService = googleAuthService;
        }
    }
    public partial class PermissionController
    {
        /// <summary>
        /// 系统所有权限列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(EntitiesListViewModel<Permission>), 0)]
        public IActionResult PermissionDictionary()
        {
            var t = PermissionDictionaryExtensions.DictPermissions.Select(i => i.Value); // 此处通过调用静态字段DictPermissions
            return new JsonResult(new EntitiesListViewModel<Permission>(t));
        }
        /// <summary>
        /// 获取当前用户被授予的角色和权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(QueryPermissionsOutViewModel), 0)]
        public IActionResult Permission(string id)
        {
            var currentId = id ?? currentUserService.CurrentUser.Id;
            if (currentId == null) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
            var targetUser = usersService.GetById(currentId);
            if (targetUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
            var permit = userActionServices.Permission(currentUserService.CurrentUser, ApplicationPermissions.User.Application.Item, PermissionType.Read, new List<string>() { targetUser.CompanyInfo.CompanyCode }, $"授权查询${currentId}权限", out var failCompany);
            var permission = context.PermissionsUsers.Where(p => p.UserId == currentId).Select(i => i.ToModel());
            var role = context.PermissionsUserRelates.Where(p => p.UserId == currentId).Select(i => i.ToModel());
            return new JsonResult(new QueryPermissionsOutViewModel() { Data = new QueryPermissionsOutDataModel() { Permissions = permission, Roles = role } });
        }
        /// <summary>
        /// 获取当前用户创建的角色和权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(EntitiesListViewModel<PermissionsRoleViewModel>), 0)]
        public IActionResult PermissionBy(string id)
        {
            var currentId = id ?? currentUserService.CurrentUser.Id;
            if (currentId == null) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
            var targetUser = usersService.GetById(currentId);
            if (targetUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
            var permit = userActionServices.Permission(currentUserService.CurrentUser, ApplicationPermissions.User.Application.Item, PermissionType.Read, new List<string>() { targetUser.CompanyInfo.CompanyCode }, $"授权查询${currentId}权限", out var failCompany);
            var role = context.PermissionsRoles.Where(p => p.CreateById == currentId).Select(i => i.Name).Distinct().ToList().Select(i => permissionServices.RoleDetail(i)).Select(i => i.Item1.ToModel(i.Item2, i.Item3, i.Item4));
            return new JsonResult(new EntitiesListViewModel<PermissionsRoleViewModel>(role));
        }
        /// <summary>
        /// 为用户赋予角色
        /// 方式1：单位主管授权，作为原始角色
        /// 方式2：使用现有角色，授权其他用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResult), 0)]
        public IActionResult RelateUserRole([FromBody] UserRalteRoleViewModel model)
        {
            var authUser = model.Auth.AuthUser(googleAuthService, usersService, currentUserService.CurrentUser.Id);
            var targetUser = usersService.GetById(model.User) ?? throw new ActionStatusMessageException(new ApiResult(new User().NotExist(), "目标用户", true));

            if (model.IsRemove)
            {
                var require_permission = permissionServices.RolePermissionCompany(model.Role);
                if (!userActionServices.Permission(authUser, ApplicationPermissions.Permissions.Role.Item, PermissionType.Write, require_permission, "取消用户权限", out var failCompany))
                    throw new ActionStatusMessageException(model.Auth.PermitDenied());
                permissionServices.UserRalteRole(targetUser.Id, model.Role, true);
            }
            else
                permissionServices.RoleRelateRole(model.Role, authUser, targetUser.Id);

            return new JsonResult(ActionStatusMessage.Success);
        }

        /// <summary>
        /// 查看角色详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(EntityViewModel<PermissionsRoleViewModel>), 0)]
        public IActionResult RoleDetail(string role)
        {
            var r = permissionServices.RoleDetail(role);
            return new JsonResult(new EntityViewModel<PermissionsRoleViewModel>(r.Item1.ToModel(r.Item2, r.Item3, r.Item4)));
        }
        /// <summary>
        /// 创建/移除角色/查看角色详情
        /// TODO 权限控制
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Role([FromBody] PermissionsRoleViewModel model)
        {
            var r = permissionServices.RoleModify(model.Role, currentUserService.CurrentUser.Id, model.IsRemove);
            if (r == null) throw new ActionStatusMessageException(r.NotExist());
            return RoleDetail(model.Role);
        }
        /// <summary>
        /// 角色关联权限/角色
        /// 主管权限可创建本单位权限
        /// 仅角色创建人可修改角色权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RolePermission([FromBody] RoleAttachPermissionViewModel model) => model.RelateRole.IsNullOrEmpty() ? RelatePermissions(model) : RelateRole(model);
        // 关联权限
        private IActionResult RelatePermissions(RoleAttachPermissionViewModel model)
        {
            var authUser = model.Auth.AuthUser(googleAuthService, usersService, currentUserService.CurrentUser.Id);
            var p = permissionServices.GetPermissionByName(model.Permission.Name) ?? throw new ActionStatusMessageException(new DAL.Entities.Permisstions.Permission().NotExist());
            if (!userActionServices.Permission(authUser, p, model.Permission.Type, new List<string>() { model.Permission.Region }, $"关联权限到{model.Role}", out var failCompany)) throw new ActionStatusMessageException(new ApiResult(model.Auth.PermitDenied(), $"授权到{p.Description}", true)); ;

            var relate_permission = permissionServices.RoleRelatePermissions(model.Role, model.Permission);
            return new JsonResult(new EntityViewModel<PermissionRoleRelatePermissionViewModel>(relate_permission.ToModel()));
        }
        // 关联角色
        private IActionResult RelateRole(RoleAttachPermissionViewModel model)
        {
            var authUser = model.Auth.AuthUser(googleAuthService, usersService, currentUserService.CurrentUser.Id);
            var require_permission = permissionServices.RolePermissionCompany(model.Role);
            if (!userActionServices.Permission(authUser, ApplicationPermissions.Permissions.Role.Item, PermissionType.Write, require_permission, "取消用户权限", out var failCompany)) throw new ActionStatusMessageException(model.Auth.PermitDenied());
            var relate_role = permissionServices.RoleRelateRole(model.Role, model.RelateRole, model.IsRemove);
            return new JsonResult(new EntityViewModel<PermissionsRoleRelateViewModel>(relate_role.ToModel()));
        }

        /// <summary>
        /// 权限鉴定
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CheckPermission([FromBody] PermissionCheckViewModel model)
        {
            var user = usersService.CurrentQueryUser(model.User);
            var result = permissionServices.CheckPermissions(user.Id, model.Permission, model.PermissionType, model.Region) ?? new PermissionBaseItem();
            userActionServices.Log(UserOperation.Permission, user.Id, $"检查权限[{model.Region}]{model.Permission}@{model.PermissionType},{result}", true);
            return new JsonResult(result == null ? ActionStatusMessage.PermissionMessage.Permission.NotExist : new EntityViewModel<IPermissionDescription>(result));
        }
    }
}