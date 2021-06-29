using Abp.Extensions;
using BLL.Extensions;
using BLL.Extensions.Common;
using BLL.Extensions.CreateClientInfo;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.Permission;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.Common;
using DAL.Entities.Permisstions;
using DAL.Entities.UserInfo;
using DAL.QueryModel;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLL.Extensions.Common.EntityModifyExtensions;

namespace BLL.Services
{
    public class UserActionServices : IUserActionServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext context;
        private readonly IUserServiceDetail userServiceDetail;
        private readonly IUsersService usersService;
        private readonly IPermissionServices permissionServices;

        public UserActionServices(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context, IUserServiceDetail userServiceDetail, IUsersService usersService, IPermissionServices permissionServices)
        {
            _httpContextAccessor = httpContextAccessor;
            this.context = context;
            this.userServiceDetail = userServiceDetail;
            this.usersService = usersService;
            this.permissionServices = permissionServices;
        }
        public UserAction Log(UserOperation operation, string username, string description, bool success = false, ActionRank rank = ActionRank.Debug)
        {
            var context = _httpContextAccessor.HttpContext;
            var ua = context.ClientInfo<UserAction>();
            ua.Date = DateTime.Now;
            ua.Operation = operation;
            ua.UserName = username;
            ua.Success = success;
            ua.Description = description;
            ua.Rank = rank;
            BackgroundJob.Enqueue<IUserActionServices>(s => s.DirectSaveUserAction(JsonConvert.SerializeObject(ua), true));
            return ua;
        }

        //public bool Permission(User authUser, DAL.Entities.Permisstions.Permission permission, PermissionType operation, string targetUserCompanyCode, string description) => Permission(authUser, permission, operation, new List<string>() { targetUserCompanyCode }, description);
        public bool Permission(User authUser, DAL.Entities.Permisstions.Permission permission, PermissionType operation, IEnumerable<string> targetUserCompanyCodes, string description, out string failCompany)
        {
            if (authUser == null) throw new ActionStatusMessageException(authUser.NotLogin());
            var authUserId = authUser.Id;
            var a = Log(UserOperation.Permission, authUserId, $"授权到[{string.Join(',', targetUserCompanyCodes)}]执行{permission?.Key}@{operation} {description}", false, ActionRank.Danger);
            var isBanTest = operation.HasFlag(PermissionType.BanRead) || operation.HasFlag(PermissionType.BanWrite);
            IEnumerable<(PermissionResult, IPermissionDescription)> result = new List<(PermissionResult, IPermissionDescription)>();
            foreach (var c in targetUserCompanyCodes)
            {
                var r = GetPermissionResult(authUser, permission, operation, c);
                if ((!isBanTest && r.Item1 == PermissionResult.Deny) || (isBanTest && r.Item1 == PermissionResult.AsDirect))
                {
                    Status(a, false, $"失败[{c}]");
                    failCompany = c;
                    return false;
                }
            }
            Status(a, true, string.Join(',', result.Select(i =>
            {
                string desc = string.Empty;
                if (i.Item2 != null)
                {
                    var t = i.Item2;
                    desc = $"{t.Name}:${t.Region}:{t.Type}";
                }
                return $"[{i.Item1}]{desc}";
            })));
            failCompany = null;
            return true;
        }
        public enum PermissionResult
        {
            Deny = 0,
            AsDirect = 1,
            AsMajor = 2,
            AsManager = 3
        }
        public (PermissionResult, IPermissionDescription) GetPermissionResult(User authUser, DAL.Entities.Permisstions.Permission permission, PermissionType operation, string targetUserCompanyCode)
        {
            if (authUser == null) return (PermissionResult.Deny, null);
            var authUserId = authUser.Id;
            targetUserCompanyCode = targetUserCompanyCode?.ToUpper() ?? string.Empty;
            var permit = permissionServices.CheckPermissions(authUserId, permission.Key, operation, targetUserCompanyCode);
            if (permit != null) return (PermissionResult.AsDirect, permit);
            var checkCompanyMajor = authUser.CheckCompanyMajor(targetUserCompanyCode);
            if (checkCompanyMajor) return (PermissionResult.AsMajor, null);
            var checkCompanyManager = authUser.CheckCompanyManager(targetUserCompanyCode, userServiceDetail);
            if (checkCompanyManager) return (PermissionResult.AsManager, null);
            return (PermissionResult.Deny, null);
        }
        public async Task<IEnumerable<UserAction>> Query(QueryUserActionViewModel model)
        {
            return await Task.Run(() =>
            {
                if (model == null) return null;
                var r = context.UserActionsDb.AsQueryable();
                if (model.UserName?.Value != null) r = r.Where(h => h.UserName == model.UserName.Value);
                if (model.Date?.Start != null && model.Date?.End != null)
                {
                    r = r.Where(h => h.Date >= model.Date.Start).Where(h => h.Date <= model.Date.End);
                }
                if (model.Rank?.Arrays != null) r = r.Where(h => model.Rank.Arrays.Contains((int)h.Rank));
                if (model.Ip?.Arrays != null) r = r.Where(h => model.Ip.Arrays.Contains(h.Ip));
                if (model.Device?.Arrays != null) r = r.Where(h => model.Device.Arrays.Contains(h.Device));
                if (model.Message?.Value != null) r = r.Where(h => h.Description.Contains(model.Message.Value));
                if (model.Page == null) model.Page = new QueryByPage()
                {
                    PageIndex = 0,
                    PageSize = 20
                };
                return r.OrderByDescending(h => h.Date).Skip(model.Page.PageSize * model.Page.PageIndex).Take(model.Page.PageSize);
            }).ConfigureAwait(true);
        }

        public UserAction Status(UserAction action, bool success, string description = null)
        {
            if (action == null) return null;
            action.Success = success;
            if (description != null) description = $"$${description}";
            action.Description = $"{action.Description}{description}";
            BackgroundJob.Enqueue<IUserActionServices>(s => s.DirectSaveUserAction(JsonConvert.SerializeObject(action), false));
            return action;
        }
        public void DirectSaveUserAction(string userActionContent, bool isAdd)
        {
            var ua = JsonConvert.DeserializeObject<UserAction>(userActionContent);
            if (isAdd)
                this.context.UserActions.Add(ua);
            else
            {
                var prev = context.UserActionsDb.FirstOrDefault(a => a.Date == ua.Date);
                if (prev != null)
                {
                    prev.Description = ua.Description;
                    prev.Success = ua.Success;
                    prev.Rank = ua.Rank;
                    prev.UserName = ua.UserName;
                    context.UserActions.Update(prev);
                }
                else
                {
                    ua.Description += "【异常】";
                    context.UserActions.Update(ua);
                }
            }

            this.context.SaveChanges();
        }
        public ApiResult LogNewActionInfo(UserAction action, ApiResult message)
        {
            Status(action, message.Status == 0, message.Message.IsNullOrEmpty() ? null : message.Message);
            return message;
        }
    }
}